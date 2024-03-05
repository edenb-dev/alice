#!/usr/bin/env python
import os
import sys
import shutil


crashed_state_directory = sys.argv[1]
stdout_file = sys.argv[2]

# Move into the crashed-state directory supplied by ALICE, and read all
# messages printed to the terminal at the time of the crash.
#os.chdir(crashed_state_directory)
#stdout = open(stdout_file).read()

dst = os.getcwd()

with open('outputfile.txt','a') as f:
	
	
	os.chdir(crashed_state_directory)
	stdout = open(stdout_file).read()
	
	#shutil.copyfile(stdout_file, dst+"/newout123.txt")
	
	#f.write(crashed_state_directory + "\t\t" + stdout_file + "\tstdout: " + stdout + "\n");



if 'Updated' in stdout:
	assert False
assert True

"""
if 'Updated' in stdout:
	# Check durability
	assert open('file1').read() == 'world'
else:
	# Check atomicity
	assert open('file1').read() in ['hello', 'world']"""