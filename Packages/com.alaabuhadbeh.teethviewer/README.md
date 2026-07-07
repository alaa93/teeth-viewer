# Teeth Viewer

A modular Unity package for viewing and revising a staged clear-aligner
treatment. A 3D arch of teeth plays through treatment stages, and individual
teeth can be selected and rotated to revise the final setup. Runs in the Unity
editor and builds to WebGL.

## Requirements

- Unity 6000.3+ with the Universal Render Pipeline (URP)

## Quick start

1. Open `teeth-viewer-demo/` as a Unity project.
2. Run **TeethViewer → Build Demo Scene** from the menu bar to generate a scene
   with a camera, light, and a full 32-tooth arch running a sample staged
   treatment.
3. Press Play.
   - **Left-click** a tooth to select it.
   - **Right-drag** a selected tooth to rotate it about its own axis, revising
     the plan's final stage live.
   - **Middle-drag** to orbit the camera; scroll to zoom.

## Architecture

```
Runtime/
  Domain/        Pure C#, no MonoBehaviour dependency. Fully unit-testable.
    ToothId              FDI-notation tooth identity (value type)
    ToothPose            Position + rotation value type, Slerp-based Lerp
    TreatmentStage       Immutable snapshot of every tooth's pose at one stage
    TreatmentPlan        Ordered stages; Evaluate() interpolates continuously;
                         WithRevisedFinalStage() applies a revision
    ToothManipulation    Ray/plane math and pixel-to-angle drag conversion
    TreatmentPlanFactory Sample staged case (no imported data needed)

  Components/    Thin MonoBehaviours — presentation and input only.
    ToothView                 Applies a ToothPose to a transform; toggles selection
    ArchLayout                Places teeth along a parabolic arch
    ToothMeshBuilder          Procedural tooth mesh generation
    TreatmentViewerController Owns the plan and drives the views each frame
    ToothSelector             Raycast selection and drag-to-revise input
    OrbitCamera               Inspection camera

  Shaders/
    Tooth.shader   URP/HLSL: half-Lambert diffuse, specular, and a fresnel
                   selection rim set per-renderer via MaterialPropertyBlock.

Editor/
  TeethViewerSceneBuilder   Menu command that assembles the demo scene.

Tests/           EditMode tests for the domain model and PlayMode tests for
                 runtime selection and raycasting.
```

The domain layer holds all logic — plan evaluation, revision, tooth identity,
and manipulation math — with no engine dependency, so it runs in fast EditMode
unit tests. The MonoBehaviour layer stays thin, keeping the package reusable
across a viewer-only build and an editing build.

## Testing

Run the suites from **Window → General → Test Runner**, or headless:

```
Unity -batchmode -runTests -testPlatform EditMode
Unity -batchmode -runTests -testPlatform PlayMode
```

## Notes

Teeth are procedurally generated and the staged movements are sample data;
there is no scan import, persistence, or clinical modeling. The project
demonstrates the rendering, math, and architecture of a treatment viewer.
