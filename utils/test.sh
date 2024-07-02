#!/bin/bash


# ------------ Validation of Script Usage ------------

# Validate arguments and path using the validation script
validate_script="$ALICE_HOME/utils/usage_validation_compile.sh"

# Source the validation script
if [ -f "$validate_script" ]; then
    source "$validate_script"
else
    echo "Error: Validation script '$validate_script' not found."
    exit 1
fi

# Check arguments and path
validate_arguments "$@"


# -------------- Compiling Project Code --------------

echo "Starting compilation."

cd $ALICE_HOME/scenario_builder
publish_output=$(dotnet publish -r linux-x64 -c Debug -p:PublishSingleFile=true -o ./public 2>&1)

# Checking compilation status.
if [ -e "./public/app" ]; then
    echo "Compilation finished successfully."
else
    echo "Error during compilation.\nCompilation output:\n$publish_output"
    exit 1
fi


# ---- Generating code ----- 

echo $DIRECTORY
echo $TASK