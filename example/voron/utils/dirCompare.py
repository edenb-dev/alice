import os # walk()
import sys # argv[]
import hashlib # hashing




def compare_directories_deep(dir_1 = None, dir_2 = None):

    # Invalid argument.
    if dir_1 is None or dir_2 is None:
        raise ValueError('(-) Argument Error - compare_directories_deep(dir_1 = None, dir_2 = None)\n\t\tplease supply directory path when using the function.')

    # Directory doesn't exist.
    if not os.path.isdir(dir_1):
        raise ValueError(f'(-) Argument Error - compare_directories_deep(dir_1 = None, dir_2 = None)\n\t\tdirectory path doesn\'t exits.\n\n\t-> given directory: {dir_1}\n')
    if not os.path.isdir(dir_2):
        raise ValueError(f'(-) Argument Error - compare_directories_deep(dir_1 = None, dir_2 = None)\n\t\tdirectory path doesn\'t exits.\n\n\t-> given directory: {dir_2}\n')

    # Same directory given.
    if dir_1 == dir_2:
        raise ValueError('(-) Argument Error - compare_directories_deep(dir_1 = None, dir_2 = None)\n\t\tdirectories path needs to differ.')


    output = []

    files = compare_directories_shallow(dir_1, dir_2)

    for file_path, indicator in files.items():

        # file exists in both directories.
        if indicator == 0:
            if (calculate_file_hash(dir_1 + file_path) != calculate_file_hash(dir_2 + file_path)):
                output.append(file_path)

    return output


def compare_directories_shallow(dir_1 = None, dir_2 = None):

    # Invalid argument.
    if dir_1 is None or dir_2 is None:
        raise ValueError('(-) Argument Error - compare_directories_shallow(dir_1 = None, dir_2 = None)\n\t\tplease supply directory path when using the function.')

    # Directory doesn't exist.
    if not os.path.isdir(dir_1):
        raise ValueError(f'(-) Argument Error - compare_directories_shallow(dir_1 = None, dir_2 = None)\n\t\tdirectory path doesn\'t exits.\n\n\t-> given directory: {dir_1}\n')
    if not os.path.isdir(dir_2):
        raise ValueError(f'(-) Argument Error - compare_directories_shallow(dir_1 = None, dir_2 = None)\n\t\tdirectory path doesn\'t exits.\n\n\t-> given directory: {dir_2}\n')
    
    # Same directory given.
    if dir_1 == dir_2:
        raise ValueError('(-) Argument Error - compare_directories_shallow(dir_1 = None, dir_2 = None)\n\t\tdirectories path needs to differ.')
    

    # files values:
    #  1 - file only in dir 1
    #  0 - file in both dirs
    # -1 - file only in dir 2
    
    files = {path[len(dir_1):]:1 for path in get_files_recursively(dir_1)}

    for path in get_files_recursively(dir_2):

        if path[len(dir_2):] in files:
            files[path[len(dir_2):]] = 0
        else:
            files[path[len(dir_2):]] = -1

    
    return files
    

def get_files_recursively(directory = None):

    # Invalid argument.
    if directory is None:
        raise ValueError('(-) Argument Error - get_files_recursively(directory = None)\n\t\tplease supply directory path when using the function.')

    # Directory doesn't exist.
    if not os.path.isdir(directory):
        raise ValueError(f'(-) Argument Error - get_files_recursively(directory = None)\n\t\tdirectory path doesn\'t exits.\n\n\t-> given directory: {directory}\n')
    

    # List of files.
    output = []
    

    for dirpath, dirs, files in os.walk(directory):
        for filename in files:
            fname = os.path.join(dirpath,filename)
            output.append(fname)

    # output, list of files paths of given the directory.
    return output


def calculate_file_hash(file = None):

    # Invalid argument.
    if file is None:
        raise ValueError('(-) Argument Error - calculate_file_hash(file = None)\n\t\tplease supply file path when using the function.')

    # File doesn't exist.
    if not os.path.isfile(file):
        raise ValueError(f'(-) Argument Error - calculate_file_hash(file = None)\n\t\tfile path doesn\'t exits.\n\n\t-> given directory: {file}\n')


    BUFFER_SIZE = 65536
  
    # Initializing the sha256() method
    sha256 = hashlib.sha256()
  
    # Opening the file.
    with open(file, 'rb') as f:

        # Reading the file
        while data := f.read(BUFFER_SIZE):
            
            # Passing data to the sh256 hash function.
            sha256.update(data)
  
    # output hash of file.
    return sha256.hexdigest()


def main():

    # Invalid Use Case.
    if len(sys.argv) != 3:
        print("(?) Supply only two directory paths you would like to compare...")
        dir_1 = input('enter first directory path: ')
        dir_2 = input('enter second directory path: ')
    else: # Updating values.
    	dir_1 = sys.argv[1]
    	dir_2 = sys.argv[2]


    # Running shallow compare.
    shallow_compare = compare_directories_shallow(dir_1, dir_2)
    deleted_files = []
    added_files = []

    for file_path, indicator in shallow_compare.items():

        # file exists in the first dir. (sys.argv[1])
        if indicator == 1:
            deleted_files.append(dir_1 + file_path)

        # file exists in the second dir. (sys.argv[2])
        elif indicator == -1:
            added_files.append(dir_2 + file_path)


    # Running deep compare.
    deep_compare = compare_directories_deep(dir_1, dir_2)
    updated_files = deep_compare


	# --------------- ALICE --------------- #
	
    print(len(deleted_files) == 0 and len(added_files) == 0 and len(updated_files) == 0)
    return
	# ------------------------------------- #


    # Printing output.
    print('\n\n------ Deleted Files ------\n')
    
    if len(deleted_files) == 0:
        print('- No files were deleted from the second directory.')
    else:
        print('(-) ' + '\n(-) '.join(deleted_files))

    print('\n------ Added Files ------\n')
    if len(added_files) == 0:
        print('- No files were added from the second directory.')
    else:
        print('(+) ' + '\n(+) '.join(added_files))

    print('\n------ Updated Files ------\n')
    if len(updated_files) == 0:
        print('- No changes or updates made to any files.')
    else:
        print('(?) ' + '\n(?) '.join(updated_files))

    # Debug
    # print(deleted_files,'\n',added_files,'\n',shallow_compare,'\n',updated_files,'\n',deep_compare)

if __name__ == "__main__":
    main()