#!/bin/bash
set -e
trap 'error ${LINENO}' ERR


rm -rf ./workload_dir
mkdir ./workload_dir

rm -rf ./traces_dir
mkdir ./traces_dir

cp -a ./librvnpal.linux.x64.so ./workload_dir/
cp -a ./libsodium.linux.x64.so ./workload_dir/
cp -a ./libzstd.linux.x64.so ./workload_dir/



# Moving into the workload directory.
cd workload_dir

# Perform the actual workload and collect traces. The "workload_dir" argument
# to alice-record specifies the entire directory which will be re-constructed
# by alice and supplied to the checker. Alice also takes an initial snapshot of
# the workload directory before beginning the workload. The "traces_dir"
# argument specifies where all the traces recorded will be stored.
alice-record --workload_dir . \
	--traces_dir ../traces_dir \
	../app

