# ProjectStar Unity Racer — Mumbai Vertical Slice

Goal: produce one 30–60 second browser racing vertical slice that visually meets or exceeds the better 3D driving games on CrazyGames, while remaining playable on modern mobile browsers.

## Current foundation
- Unity 6 project shell
- URP rendering pipeline
- GameSpec-driven runtime configuration
- Mumbai monsoon race spec
- Editor scene builder
- WebGL build entry point
- Mobile and desktop FPS targets

## Visual bar
Do not approve placeholder geometry. Visual review begins only after these are installed:
1. High-detail licensed vehicle with correct wheel hierarchy, glass and PBR materials
2. Tiled wet asphalt PBR set: albedo, normal, roughness and mask maps
3. Mumbai streetscape kit: facades, flyovers, palms, barriers, signage and street furniture
4. Traffic kit: taxis, buses, rickshaws and civilian cars
5. Monsoon sky/HDR environment and authored lighting
6. Rain, tyre spray, reflections, contact shadows and atmospheric VFX
7. Cinematic chase camera and mobile touch controls

## Success gate
- Better than the old Three.js benchmark immediately on first frame
- Coherent art direction, not primitive/procedural city blocks
- Stable target: 45 FPS mobile, 60 FPS desktop where possible
- Build size and texture memory measured before adding more content

## Build
Open the Unity project and run:
- ProjectStar > Build Mumbai Vertical Slice
- ProjectStar > Build WebGL Vertical Slice

Cloud build should invoke `ProjectStar.Racer.Editor.WebGLBuild.Build`.

## Architecture rule
AI configures and assembles a professionally authored template through GameSpec. It does not generate the runtime world from ad-hoc geometry every time.
