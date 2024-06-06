cmake_minimum_required(VERSION 3.15)

project(DataImplementation)
# set(CMAKE_DOTNET_TARGET_FRAMEWORK_VERSION "v8.0")
file(GLOB_RECURSE CS_FILES "*.cs")

add_custom_target(DataImplementation ALL COMMAND dotnet build ${CMAKE_CURRENT_SOURCE_DIR} -c Release SOURCES ${CS_FILES})