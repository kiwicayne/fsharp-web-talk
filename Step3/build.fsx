// Include Fake lib
#r @"packages/FAKE/tools/FakeLib.dll"
open Fake
open Fake.FscHelper

// Properties
let buildDir = "./build"
let packagesDir = "./packages"

// Targets
Target "Clean" (fun _ ->
    CleanDir buildDir
)

Target "RestorePackages" (fun _ ->
  "MessageIt.sln"
    |> RestoreMSSolutionPackages (fun p ->
         { p with             
             OutputPath = packagesDir
             Retries = 4 })
)

Target "BuildSolution" (fun _ ->
  ["MessageIt.sln"]
    |> MSBuildRelease buildDir "Rebuild"
    |> Log "AppBuild-Output: "
)

Target "Default" (fun _ ->
    trace "Starting Default target..."
)

// Dependencies
"Clean"
  ==> "RestorePackages"
  ==> "BuildSolution"
  ==> "Default"
    
// Start build
RunTargetOrDefault "Default"