#!/bin/bash


# Function to display usage information
usage() {
    echo "Usage: $0 [-t|-c] <dir_name>"
    echo "  -t : Compiles a test and creates a workload script."
    echo "  -c : Compiles a checker script."
    exit 1
}

# Function to validate arguments
validate_arguments() {

    if [ "$#" -ne 2 ]; then
        usage
    fi

    while getopts ":tc:" opt; do
        case $opt in
            t)
                TASK="T"
                ;;
            c)
                TASK="C"
                ;;
            :)
                echo "Option -$OPTARG requires an argument."
                usage
                ;;
            \?)
                echo "Invalid option: -$OPTARG"
                usage
                ;;
        esac
    done


    # Check if TASK is not set (implies no -t or -c)
    if [ -z "$TASK" ]; then
        echo "Error: You must specify either -t or -c option."
    usage
    fi

    shift $((OPTIND - 1))
    DIRECTORY="$1"
}