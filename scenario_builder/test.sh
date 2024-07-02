#!/bin/bash

# Check if exactly 2 arguments are provided
if [ $# -ne 2 ]
then
  echo "Usage: $0 <param1> <param2>"
  exit 1
fi

# Assign arguments to variables
param1="$1"
param2="$2"

# Do something with the parameters
echo "Parameter 1: $param1"
echo "Parameter 2: $param2"

# You can replace this echo with your desired functionality
# For example, process a file, perform calculations, etc.

# exit 0

