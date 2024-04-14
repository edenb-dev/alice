#!/usr/bin/env python
import os
import sys



# Current dir. (pwd)
dst = os.getcwd()



# ----- Alice (required) -----
crashed_state_directory = sys.argv[1]
stdout_file = sys.argv[2]

# Move into the crashed-state directory supplied by ALICE, and read all
# messages printed to the terminal at the time of the crash.
os.chdir(crashed_state_directory)
stdout = open(stdout_file).read()
# ----- Alice (required) -----



# Saving print output.
with open(dst+'/print.txt','a') as file:
	
	file.write(crashed_state_directory + "\t\t\tstdout: " + stdout + "\n");