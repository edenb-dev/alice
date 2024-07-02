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

# DEBUG
# echo $DIRECTORY
# echo $TASK

mkdir -p $ALICE_HOME/workspace/$DIRECTORY

if [ "$TASK" = "T" ]; then
    mv "$ALICE_HOME/scenario_builder/public/app" "$ALICE_HOME/workspace/$DIRECTORY/test"
    cp -a "$ALICE_HOME/workspace/$DIRECTORY/test" "$ALICE_HOME/workspace/app"

    cp -a "$ALICE_HOME/workspace/global_workload.sh" "$ALICE_HOME/workspace/$DIRECTORY/workload.sh"
elif [ "$TASK" = "C" ]; then
    mv "$ALICE_HOME/scenario_builder/public/app" "$ALICE_HOME/workspace/$DIRECTORY/checker"
fi




if [ "$TASK" = "T" ]; then
    echo "Running workload in workspace dir"

    cd $ALICE_HOME/workspace
    $ALICE_HOME/workspace/workload.sh
fi








