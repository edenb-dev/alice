#!/usr/bin/env python
import os
import sys

from os.path import exists


"""
# Current dir. (pwd)
dst = os.getcwd()
"""


# ----- Alice (required) -----
crashed_state_directory = sys.argv[1]
stdout_file = sys.argv[2]

# Move into the crashed-state directory supplied by ALICE, and read all
# messages printed to the terminal at the time of the crash.
#os.chdir(crashed_state_directory)
stdout = open(stdout_file).read()
# ----- Alice (required) -----


#print('wwowowowowwo')
"""
if "test1" in stdout:
	assert True
else 
	assert False
"""




# Saving print output.
with open('./print.txt','a') as file:
	
	file_exists = exists(crashed_state_directory)
	
	file.write("\n" +str(file_exists) + "\n")
	file.write("\n"+crashed_state_directory+ "\n" + stdout_file + "\n" + stdout)
	#file.write(crashed_state_directory + "\t\t\tstdout: " + stdout + "\n");