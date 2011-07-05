def test_python26_build_map():
	import sys
	
	if '.'.join(str(x) for x in sys.version_info[:2]) == '2.6':
		from byteplay import Code
		Code.from_code((lambda: {'a': 1}).func_code).to_code()
