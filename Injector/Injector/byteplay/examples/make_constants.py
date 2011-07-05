# Decorator for BindingConstants at compile time
# Based on a recipe by Raymond Hettinger, from Python Cookbook:
# http://aspn.activestate.com/ASPN/Cookbook/Python/Recipe/277940
#
# Modified by Noam Raphael to demonstrate using the byteplay module
# (http://code.google.com/p/byteplay)

from byteplay import *

def _make_constants(f, builtin_only=False, stoplist=[], verbose=False):
    try:
        co = f.func_code
    except AttributeError:
        return f        # Jython doesn't have a func_code attribute.
    
    co = Code.from_code(co)

    import __builtin__
    env = vars(__builtin__).copy()
    if builtin_only:
        stoplist = dict.fromkeys(stoplist)
        stoplist.update(f.func_globals)
    else:
        env.update(f.func_globals)

    # First pass converts global lookups into constants
    for i, (op, arg) in enumerate(co.code):
        if op == LOAD_GLOBAL:
            name = arg
            if name in env and name not in stoplist:
                value = env[name]
                co.code[i] = (LOAD_CONST, value)
                if verbose:
                    print name, '-->', value

    # Second pass folds tuples of constants and constant attribute lookups
    newcode = []
    constcount = 0
    NONE = [] # An object that won't appear anywhere else
    for op, arg in co.code:
        newconst = NONE
        if op == LOAD_CONST:
            constcount += 1
        elif op == LOAD_ATTR:
            if constcount >= 1:
                lastop, lastarg = newcode.pop()
                constcount -= 1
                newconst = getattr(lastarg, arg)
        elif op == BUILD_TUPLE:
            if constcount >= arg:
                newconst = tuple(x[1] for x in newcode[-1:-1-arg:-1])
                del newcode[-arg:]
                constcount -= arg
        else:
            constcount = 0

        if newconst is not NONE:
            newcode.append((LOAD_CONST, newconst))
            constcount += 1
            if verbose:
                print "new folded constant:", newconst
        else:
            newcode.append((op, arg))
    co.code = newcode
    
    return type(f)(co.to_code(), f.func_globals, f.func_name, f.func_defaults,
                   f.func_closure)

_make_constants = _make_constants(_make_constants) # optimize thyself!

def bind_all(mc, builtin_only=False, stoplist=[],  verbose=False):
    """Recursively apply constant binding to functions in a module or class.

    Use as the last line of the module (after everything is defined, but
    before test code).  In modules that need modifiable globals, set
    builtin_only to True.

    """
    try:
        d = vars(mc)
    except TypeError:
        return
    for k, v in d.items():
        if type(v) is FunctionType:
            newv = _make_constants(v, builtin_only, stoplist,  verbose)
            setattr(mc, k, newv)
        elif type(v) in (type, ClassType):
            bind_all(v, builtin_only, stoplist, verbose)

@_make_constants
def make_constants(builtin_only=False, stoplist=[], verbose=False):
    """ Return a decorator for optimizing global references.

    Replaces global references with their currently defined values.
    If not defined, the dynamic (runtime) global lookup is left undisturbed.
    If builtin_only is True, then only builtins are optimized.
    Variable names in the stoplist are also left undisturbed.
    Also, folds constant attr lookups and tuples of constants.
    If verbose is True, prints each substitution as is occurs

    """
    if type(builtin_only) == type(make_constants):
        raise ValueError("The bind_constants decorator must have arguments.")
    return lambda f: _make_constants(f, builtin_only, stoplist, verbose)

## --------- Example call -----------------------------------------

import random

@make_constants(verbose=True)
def sample(population, k):
    "Choose k unique random elements from a population sequence."
    if not ininstance(population, (list, tuple, str)):
        raise TypeError('Cannot handle type', type(population))
    n = len(population)
    if not 0 <= k <= n:
        raise ValueError, "sample larger than population"
    result = [None] * k
    pool = list(population)
    for i in xrange(k):         # invariant:  non-selected at [0,n-i)
        j = int(random.random() * (n-i))
        result[i] = pool[j]
        pool[j] = pool[n-i-1]   # move non-selected item into vacancy
    return result

""" Output from the example call:

list --> <type 'list'>
tuple --> <type 'tuple'>
str --> <type 'str'>
TypeError --> exceptions.TypeError
type --> <type 'type'>
len --> <built-in function len>
ValueError --> exceptions.ValueError
list --> <type 'list'>
xrange --> <type 'xrange'>
int --> <type 'int'>
random --> <module 'random' from 'C:\PYTHON24\lib\random.pyc'>
new folded constant: (<type 'list'>, <type 'tuple'>, <type 'str'>)
new folded constant: <built-in method random of Random object at 0x00A281E8>
"""
