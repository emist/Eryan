#!/usr/bin/python

from setuptools import setup, find_packages
from byteplay import __version__ as lib_version

setup(
       name = 'byteplay',
       author='Noam Raph',
       author_email='noamraph@gmail.com',
       url='http://code.google.com/p/byteplay',
       download_url='http://code.google.com/p/byteplay/downloads/list',
       version = lib_version,
       py_modules = ['byteplay'],
       zip_safe = True,
       license='LGPL',
       description='bytecode manipulation library',
       long_description = """byteplay lets you convert Python code objects into equivalent objects which are easy to play with, and lets you convert those objects back into living Python code objects. It's useful for applying crazy transformations on Python functions, and is also useful in learning Python byte code intricacies. It currently works with Python 2.4 and up.

byteplay Module Documentation
=============================

About byteplay
--------------

byteplay is a module which lets you easily play with Python bytecode. I wrote
it because I needed to manipulate Python bytecode, but didn't find any suitable
tool. Michael Hudson's bytecodehacks (http://bytecodehacks.sourceforge.net/)
could have worked fine for me, but it only works with Python 1.5.2. I also
looked at Phillip J. Eby's peak.util.assembler
(http://pypi.python.org/pypi/BytecodeAssembler), but it's intended at
creating new code objects from scratch, not for manipulating existing code
objects.

So I wrote byteplay. The basic idea is simple: define a new type, named Code,
which is equivalent to Python code objects, but, unlike Python code objects, is
easy to play with. "Equivalent" means that every Python code object can be
converted to a Code object and vice-versa, without losing any important
information on the way. "Easy to play with" means... well, exactly that. The
representation should be as simple as possible, letting the infrastructure sort
out technical details which do not affect the final behaviour.

If you are interested in changing the behaviour of functions, or in assembling
functions on your own, you may find byteplay useful. You may also find it useful
if you are interested in how Python's bytecode actually works - byteplay lets
you easily play with existing bytecode and see what happens, which is a great
way to learn. You are also welcome to check byteplay's (pure Python) code, to
see how it manipulates real bytecode.

byteplay can be downloaded from http://byteplay.googlecode.com/svn/trunk/byteplay.py . See http://code.google.com/p/byteplay/ for a bit more administrative info.

Feel free to improve this document - that's why it's on the wiki! Also, if you find it useful, please drop me an email at noamraph at gmail dot com - it would be nice knowing that what I did was useful to someone...

A Quick Example
---------------

Let's start from a quick example, to give you a taste of what byteplay does.
Let's define this stupid function::

    >>> def f(a, b):
    ...     print (a, b)
    ...
    >>> f(3, 5)
    (3, 5)

Now, let's use byteplay to see what its bytecode is actually doing::

    >>> from byteplay import *
    >>> from pprint import pprint
    >>> c = Code.from_code(f.func_code)
    >>> pprint(c.code)
    [(SetLineno, 2),
     (LOAD_FAST, 'a'),
     (LOAD_FAST, 'b'),
     (BUILD_TUPLE, 2),
     (PRINT_ITEM, None),
     (PRINT_NEWLINE, None),
     (LOAD_CONST, None),
     (RETURN_VALUE, None)]

I hope that this is pretty clear if you are a bit familiar with bytecode. The
Code object contains a list of all operations, which are pairs of (opcode,
arg). Not all opcodes have an argument, so they have None as their argument. You
can see that no external tables are used: in the raw bytecode, the argument of
many opcodes is an index to a table - for example, the argument of the
LOAD_CONST opcode is an index to the co_consts table, which contains the actual
constants. Here, the argument is the constant itself. Also note the SetLineno
"opcode". It is not a real opcode, but it is used to declare where a line in the
original source code begins. Besides another special opcode defined by byteplay,
which we will see later, all other opcodes are the real opcodes used by the
Python interpreter.

By the way, if you want to see the code list in a form which is
easier to read, you can simply print it, like this::

    >>> print c.code

      2           1 LOAD_FAST            a
                  2 LOAD_FAST            b
                  3 BUILD_TUPLE          2
                  4 PRINT_ITEM
                  5 PRINT_NEWLINE
                  6 LOAD_CONST           None
                  7 RETURN_VALUE

This is especially useful if the code contains jumps. See the
description of the printcodelist function for another example.

Ok, now let's play! Say we want to change the function, to print its arguments
in reverse order. To do this, we will add a ROT_TWO opcode after the two
arguments were loaded to the stack. See how simple it is::

    >>> c.code[3:3] = [(ROT_TWO, None)]
    >>> f.func_code = c.to_code()
    >>> f(3, 5)
    (5, 3)


Opcodes
-------

We have seen that the code list contains opcode constants such as LOAD_FAST.
These are instances of the Opcode class. The Opcode class is a subclass of int,
which overrides the ``__repr__`` method to return the string representation of
an opcode. This means that instead of using a constant such as LOAD_FAST, a
numerical constant such as 124 can be used. Opcode instances are, of course,
much easier to understand. The byteplay module creates Opcode instances for all
the interpreter opcodes. They can be found in the ``opcodes`` set, and also in
the module's global namespace, so you can write ``from byteplay import *`` and
use the opcode constants immediately.

byteplay doesn't include a constant for the EXTENDED_ARG opcode, as it is not
used by byteplay's representation.


Module Contents
---------------

These are byteplay's public attributes, which are imported when ``from byteplay
import *`` is done.

``POP_TOP``, ``ROT_TWO``, etc.
  All bytecode constants are imported by their names.

``opcodes``
  A set of all Opcode instances.

``opmap``
  A mapping from an opcode name to an Opcode instance.

``opname``
  A mapping from an opcode number (and an Opcode instance) to its name.

``cmp_op``
  A list of strings which represent comparison operators. In raw bytecode, the
  argument of the COMPARE_OP opcode is an index to this list. In the code list,
  it is the string representing the comparison.

The following are sets of opcodes, which list opcodes according to their
behaviour.

``hasarg``
  This set contains all opcodes which have an argument (these are the opcodes
  which are >= HAVE_ARGUMENT).

``hasname``
  This set contains all opcodes whose argument is an index to the co_names list.

``hasjrel``
  This set contains all opcodes whose argument is a relative jump, that is, an
  offset by which to advance the byte code instruction pointer.

``hasjabs``
  This set contains all opcodes whose argument is an absolute jump, that is, an
  address to which the instruction pointer should jump.

``hasjump``
  This set contains all opcodes whose argument is a jump. It is simply
  ``hasjrel + hasjabs``. In byteplay, relative and absolute jumps behave in the
  same way, so this set is convenient.

``haslocal``
  This set contains all opcodes which operate on local variables.

``hascompare``
  This set contains all opcodes whose argument is a comparison operator - that
  is, only the COMPARE_OP opcode.

``hasfree``
  This set contains all opcodes which operate on the cell and free variable
  storage. These are variables which are also used by an enclosing or an
  enclosed function.

``hascode``
  This set contains all opcodes which expect a code object to be at the top of
  the stack. In the bytecode the Python compiler generates, they are always
  preceded by a LOAD_CONST opcode, which loads the code object.

``hasflow``
  This set contains all opcodes which have a special flow behaviour. All other
  opcodes always continue to the next opcode after finished, unless an exception
  was raised.

The following are the types of the first elements of the opcode list tuples.

``Opcode``
  The type of all opcode constants.

``SetLineno``
  This singleton is used like the "real" opcode constants, but only declares
  where starts the bytecode for a specific line in the source code.

``Label``
  This is the type of label objects. This class does nothing - it is used as a
  way to refer to a place in the code list.

Here come some additional functions.

``isopcode(obj)``
  Use this function to check whether the first element of an operation pair is
  a real opcode. This simply returns ``obj is not SetLineno and not
  isinstance(obj, Label)``.

``getse(op[, arg])``
  This function gets the stack effect of an opcode, as a (pop, push) tuple. The
  stack effect is the number of items popped from the stack, and the number of
  items pushed instead of them. If an item is only inspected, it is considered
  as though it was popped and pushed again. This function is meaningful only
  for opcodes not in hasflow - for other opcodes, ValueError will be raised.

  For some opcodes the argument is needed in order to calculate the stack
  effect. In that case, if arg isn't given, ValueError will be raised.

``printcodelist(code, to=sys.stdout)``
  This function gets a code list and prints it in a way easier to read. For
  example, let's define a simple function::

    >>> def f(a):
    ...     if a < 3:
    ...         b = a
    ...
    >>> c = Code.from_code(f.func_code)

  This is the code list itself::

    >>> pprint(c.code)
    [(SetLineno, 2),
     (LOAD_FAST, 'a'),
     (LOAD_CONST, 3),
     (COMPARE_OP, '<'),
     (JUMP_IF_FALSE, <byteplay.Label object at 0xb7c6a16c>),
     (POP_TOP, None),
     (SetLineno, 3),
     (LOAD_FAST, 'a'),
     (STORE_FAST, 'b'),
     (JUMP_FORWARD, <byteplay.Label object at 0xb7c6a18c>),
     (<byteplay.Label object at 0xb7c6a16c>, None),
     (POP_TOP, None),
     (<byteplay.Label object at 0xb7c6a18c>, None),
     (LOAD_CONST, None),
     (RETURN_VALUE, None)]

  And this is the nicer representation::

    >>> printcodelist(c.code)

      2           1 LOAD_FAST            a
                  2 LOAD_CONST           3
                  3 COMPARE_OP           <
                  4 JUMP_IF_FALSE        to 11
                  5 POP_TOP

      3           7 LOAD_FAST            a
                  8 STORE_FAST           b
                  9 JUMP_FORWARD         to 13
            >>   11 POP_TOP
            >>   13 LOAD_CONST           None
                 14 RETURN_VALUE

  As you can see, all opcodes are marked by their index in the list,
  and jumps show the index of the target opcode.

For your convenience, another class was defined:

``CodeList``
  This class is a list subclass, which only overrides the __str__ method to
  use ``printcodelist``. If the code list is an instance of CodeList, you don't
  have to type ``printcodelist(c.code)`` in order to see the nice
  representation - just type ``print c.code``. Code instances created from
  raw Python code objects already have that feature!

And, last but not least - the Code class itself!


The Code Class
--------------

Constructor
~~~~~~~~~~~

::

  Code(code, freevars, args, varargs, varkwargs, newlocals,
       name, filename, firstlineno, docstring) -> new Code object

This constructs a new Code object. The argument are simply values for the
Code object data attributes - see below.

Data Attributes
~~~~~~~~~~~~~~~

We'll start with the data attributes - those are read/write, and distinguish
one code instance from another. First come the attributes which affect the
operation of the interpreter when it executes the code, and then come attributes
which give extra information, useful for debugging and introspection.

``code``
  This is the main part which describes what a Code object does. It is a list
  of pairs ``(opcode, arg)``. ``arg`` is the opcode argument, if it has one, or
  None if it doesn't. ``opcode`` can be of 3 types:

  * Regular opcodes. These are the opcodes which really define an operation of
    the interpreter. They can be regular ints, or Opcode instances. The
    meaning of the argument changes according to the opcode:

    - Opcodes not in ``hasarg`` don't have an argument. None should be used as
      the second item of the tuple.
    - The argument of opcodes in ``hasconst`` is the actual constant.
    - The argument of opcodes in ``hasname`` is the name, as a string.
    - The argument of opcodes in ``hasjump`` is a Label instance, which should
      point to a specific location in the code list.
    - The argument of opcodes in ``haslocal`` is the local variable name, as
      a string.
    - The argument of opcodes in ``hascompare`` is the string representing the
      comparison operator.
    - The argument of opcodes in ``hasfree`` is the name of the cell or free
      variable, as a string.
    - The argument of the remaining opcodes is the numerical argument found in
      raw bytecode. Its meaning is opcode specific.

  * ``SetLineno``. This is a singleton, which means that a line in the source
    code begins. Its argument is the line number.

  * labels. These are instances of the ``Label`` class. The label class does
    nothing - it is just used as a way to specify a place in the code list.
    Labels can be put in the code list and cause no action by themselves.
    They are used as the argument of opcodes which may cause a jump to a
    specific location in the code.

``freevars``
  This is a list of strings - the names of variables defined in outer functions
  and used in this function or in functions defined inside it. The order of this
  list is important, since those variables are passed to the function as a
  sequence whose order should match the order of the ``freevars`` attribute.

  A few words about closures in Python may be in place. In Python, functions
  defined inside other functions can use variables defined in an outer function.
  We know each running function has a place to store local variables. But how
  can functions refer to variables defined in an outer scope?

  The solution is this: for every variable which is used in more than one scope,
  a new ``cell`` object is created. This object does one simple thing: it refers
  to one another object - the value of its variable. When the variable gets a
  new value, the cell object is updated too. A reference to the cell object is
  passed to any function which uses that variable. When an inner function is
  interested in the value of a variable of an outer scope, it uses the value
  referred by the cell object passed to it.

  An example might help understand this. Let's take a look at the bytecode of a
  simple example::

    >>> def f():
    ...     a = 3
    ...     b = 5
    ...     def g():
    ...         return a + b
    ...
    >>> from byteplay import *
    >>> c = Code.from_code(f.func_code)
    >>> print c.code

      2           1 LOAD_CONST           3
                  2 STORE_DEREF          a

      3           4 LOAD_CONST           5
                  5 STORE_DEREF          b

      4           7 LOAD_CLOSURE         a
                  8 LOAD_CLOSURE         b
                  9 BUILD_TUPLE          2
                 10 LOAD_CONST           <byteplay.Code object at 0xb7c6a56c>
                 11 MAKE_CLOSURE         0
                 12 STORE_FAST           g
                 13 LOAD_CONST           None
                 14 RETURN_VALUE

    >>> c.code[10][1].freevars
    ('a', 'b')
    >>> print c.code[10][1].code

      5           1 LOAD_DEREF           a
                  2 LOAD_DEREF           b
                  3 BINARY_ADD
                  4 RETURN_VALUE

  We can see that LOAD_DEREF and STORE_DEREF opcodes are used to get and set the
  value of cell objects. There is no inherent difference between cell objects
  created by an outer function and cell objects used in an inner function. What
  makes the difference is whether a variable name was listed in the ``freevars``
  attribute of the Code object - if it was not listed there, a new cell is
  created, and if it was listed there, the cell created by an outer function
  is used.

  We can also see how a function gets the cell objects it needs from its outer
  functions. The inner function is created with the MAKE_CLOSURE opcode, which
  pops two objects from the stack: first, the code object used to create the
  function. Second, a tuple with the cell objects used by the code (the tuple
  is created by the LOAD_CLOSURE opcodes, which push a cell object into the
  stack, and of course the BUILD_TUPLE opcode.) We can see that the order of the
  cells in the tuple match the order of the names in the ``freevars`` list -
  that's how the inner function knows that ``(LOAD_DEREF, 'a')`` means "load
  the value of the first cell in the tuple".

``args``
  The list of arguments names of a function. For example::

    >>> def f(a, b, *args, **kwargs):
    ...     pass
    ...
    >>> Code.from_code(f.func_code).args
    ('a', 'b', 'args', 'kwargs')

``varargs``
  A boolean: Does the function get a variable number of positional arguments?
  In other words: does it have a ``*args`` argument?

  If ``varargs`` is True, the argument which gets that extra positional
  arguments will be the last argument or the one before the last, depending
  on whether ``varkwargs`` is True.

``varkwargs``
  A boolean: Does the function get a variable number of keyword arguments?
  In other words: does it have a ``**kwargs`` argument?

  If ``varkwargs`` is True, the argument which gets the extra keyword arguments
  will be the last argument.

``newlocals``
  A boolean: Should a new local namespace be created for this code? This
  is True for functions and False for modules and exec code.

Now come attributes with additional information about the code:

``name``
  A string: The name of the code, which is usually the name of the function
  created from it.

``filename``
  A string: The name of the source file from which the bytecode was compiled.

``firstlineno``
  An int: The number of the first line in the source file from which the
  bytecode was compiled.

``docstring``
  A string: The docstring for functions created from this code.


Methods
~~~~~~~

These are the Code class methods.

``Code.from_code(code) -> new Code object``
  This is a static method, which creates a new Code object from a raw Python
  code object. It is equivalent to the raw code object, that is, the resulting
  Code object can be converted to a new raw Python code object, which will
  have exactly the same behaviour as the original object.

``code.to_code() -> new code object``
  This method converts a Code object into an equivalent raw Python code object,
  which can be executed just like any other code object.

``code1.__eq__(code2) -> bool``
  Different Code objects can be meaningfully tested for equality. This tests
  that all attributes have the same value. For the code attribute, labels are
  compared to see if they form the same flow graph.


Stack-depth Calculation
-----------------------

What was described above is enough for using byteplay. However, if you encounter
an "Inconsistent code" exception when you try to assemble your code and wonder
what it means, or if you just want to learn more about Python's stack
behaviour, this section is for you.

Note: This section isn't as clear as it could have been, to say the least. If
you like to improve it, feel free to do so - that's what wikis are for, aren't
they?

When assembling code objects, the code's maximum stack usage is needed. This is
simply the maximum number of items expected on the frame's stack. If the actual
number of items in stack exceeds this, Python may well fail with a segmentation
fault. The question is then, how to calculate the maximum stack usage of a
given code?

There's most likely no general solution for this problem. However, code
generated by Python's compiler has a nice property which makes it relatively
easy to calculate the maximum stack usage. The property is that if we take a
bytecode "line", and check the stack state whenever we reach that line, we will
find the stack state when we reach that line is always the same, no matter how
we got to that line. We'll call such code "regular".

Now, this requires clarification: what is the "stack state" which is always the
same, exactly? Obviously, the stack doesn't always contain the same objects
when we reach a line. For now, we can assume that it simply means the number of
items on the stack.

This helps us a lot. If we know that every line can have exactly one stack
state, and we know how every opcode changes the stack state, we can trace stack
states along all possible code paths, and find the stack state of every
reachable line. Then we can simply check what state had the largest number of
stack items, and that's the maximum stack usage of the code. What will happen
with code not generated by Python's compiler, if it doesn't fulfill the
requirement that every line should have one state? When tracing the stack state
for every line, we will find a line, which can be reached from several places,
whose stack state changes according to the address from which we jumped to that
line. In that case, An "Inconsistent code" exception will be raised.

Ok, what is really what we called "stack state"? If every opcode pushed and
popped a constant number of elements, the stack state could have been the
number of items on stack. However, life isn't that simple. In real life, there
are *blocks*. Blocks allow us to break from a loop, regardless of exactly how
many items we have in stack. How? Simple. Before the loop starts, the
SETUP_LOOP opcode is executed. This opcode records in a block the number of operands(items)
currently in stack, and also a position in the code. When the POP_BLOCK is executed, the stack is restored to the recorded state by poping extra items, and the corresponding block is
discarded. But if the BREAK_LOOP opcode is
executed instead of POP_BLOCK, one more thing happens. The
execution jumps to the position specified by the SETUP_LOOP opcode.

Fortunately, we can still live with that. Instead of defining the stack state
as a single number - the total number of elements in the stack, we will define
the stack state as a sequence of numbers - the number of elements in the stack
per each block. So, for example, if the state was (3, 5), after a BINARY_ADD
operation the state will be (3, 4), because the operation pops two elements and
pushes one element. If the state was (3, 5), after a PUSH_BLOCK operation the
state will be (3, 5, 0), because a new block, without elements yet, was pushed.

Another complication: the SETUP_FINALLY opcode specifies an address to jump to
if an exception is raised or a BREAK_LOOP operation was executed. This address
can also be reached by normal flow. However, the stack state in that address
will be different, depending on what actually happened - if an exception was
raised, 3 elements will be pushed, if BREAK_LOOP was executed, 2 elements will
be pushed, and if nothing happened, 1 element will be pushed by a LOAD_CONST
operation. This seemingly non-consistent state always ends with an
END_FINALLY opcode. The END_FINALLY opcodes pops 1, 2 or 3 elements according to
what it finds on stack, so we return to "consistent" state. How can we deal
with that complexity?

The solution is pretty simple. We will treat the SETUP_FINALLY opcode as
if it pushes 1 element to its target - this makes it consistent with the 1
element which is pushed if the target is reached by normal flow. However,
we will calculate the stack state as if at the target line there was an opcode
which pushed 2 elements to the stack. This is done so that the maximum stack
size calculation will be correct. Those 2 extra elements will be popped by the
END_FINALLY opcode, which will be treated as though it always pops 3 elements.
That's all! Just be aware of that when you are playing with
SETUP_FINALLY and END_FINALLY opcodes...

       """
)
