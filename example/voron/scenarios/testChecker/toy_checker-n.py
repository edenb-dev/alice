#!/usr/bin/env python
import os
import sys
import subprocess




def main():
	
	# ----- Alice (required) -----
	crashed_state_directory = sys.argv[1]
	stdout_file = sys.argv[2]

	# Move into the crashed-state directory supplied by ALICE, and read all
	# messages printed to the terminal at the time of the crash.
	os.chdir(crashed_state_directory)
	stdout = open(stdout_file).read()
	# ----- Alice (required) -----
	
	
	"""
	if 'Changes commited successfully.' in stdout:

		# Check durability
		assert compareDatabaseInstances('./CurrentDatabaseState','./FinalDatabaseState')
	else:
		# Check atomicity
		assert compareDatabaseInstances('./CurrentDatabaseState','./InitialDatabaseState') or compareDatabaseInstances('./CurrentDatabaseState','./FinalDatabaseState')
	"""

	#assert compareDatabaseInstances('./FinalDatabaseState','./FinalDatabaseStateCopy')



def compareDatabaseInstances(path1,path2):

	p = subprocess.Popen(['python3', os.environ['ALICE_HOME'] + '/example/voron/utils/dirCompare.py', path1, path2], stdout=subprocess.PIPE, stderr=subprocess.PIPE)
	out, err = p.communicate()

	return out.strip() == 'True'



main()