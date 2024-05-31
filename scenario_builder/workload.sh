#!/bin/bash
set -e
trap 'error ${LINENO}' ERR

dotnet build

cd compiled/test1/initial/

# The workload directory is where the files of the application will be stored.
# The application, as it runs, will modify the workload directory and its contents.
rm -rf workload_dir
mkdir workload_dir

# The traces directory is for storing the (multiple) traces that are recorded
# as the application is run.
rm -rf traces_dir
mkdir traces_dir



# Compiling the example application.
# gcc -g -fPIC HelloWorld.c -o HelloWorld
# This is a C# example. We currently precompile it.


cp -a ./librvnpal.linux.x64.so workload_dir/
cp -a ./libsodium.linux.x64.so workload_dir/
cp -a ./libzstd.linux.x64.so workload_dir/




# Moving into the workload directory.
cd workload_dir


# Perform the actual workload and collect traces. The "workload_dir" argument
# to alice-record specifies the entire directory which will be re-constructed
# by alice and supplied to the checker. Alice also takes an initial snapshot of
# the workload directory before beginning the workload. The "traces_dir"
# argument specifies where all the traces recorded will be stored.
alice-record --workload_dir . \
	--traces_dir ../traces_dir \
	/home/eden/Projects/Final_Project_RavenDB/alice/scenario_builder/bin/Debug/net7.0/app
