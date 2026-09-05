# ProjectStar Unity Racer — Mumbai Vertical Slice

Goal: produce a 30–60 second browser racing vertical slice that reaches the better 3D driving-game tier on CrazyGames and remains playable on modern mobile browsers.

## Current build stage — Benchmark 01
- Unity 6 / URP / WebGL cloud pipeline proven
- playable rigidbody driving controller
- speed-sensitive steering and chase camera
- iPhone touch zones + desktop keyboard controls
- 4-lane wet Marine Drive-inspired road
- night/monsoon lighting, fog, bloom, contrast and vignette
- rain particles and moving traffic
- streetlights, Art Deco-inspired frontage, promenade, seawall, palms, signage and start gantry
- 45 FPS mobile / 60 FPS desktop targets

## Benchmark gate
Benchmark references: better current CrazyGames 3D driving titles such as Traffic Racer, Real Cars in City and Real Car Driving.

We score each test build from 1–5 on:
1. first-frame visual credibility
2. vehicle feel / steering stability
3. chase-camera smoothness
4. road/material credibility
5. environment density and coherent scale
6. lighting/atmosphere
7. mobile control responsiveness
8. frame pacing on iPhone Safari
9. load time / WebGL stability

**Acceptance:** no category below 3; average 3.7 or better before this becomes `racer-v1`.

## Important visual rule
Benchmark 01 establishes gameplay, camera, atmosphere and scene-density behaviour using generated runtime geometry. It is not the final art pass. The production visual pass must replace the generated vehicle/facades with professionally authored licensed assets.

Required production assets:
- high-detail vehicle with proper wheel hierarchy, glass and PBR materials
- tiled wet asphalt PBR maps
- authored Mumbai streetscape/landmark modules
- traffic vehicles including taxis/buses/rickshaws
- HDR environment + calibrated lighting
- tyre spray/contact VFX and optimized reflections

## Asset licensing direction
Kenney Racing Kit and City Kits are confirmed CC0 and are safe fallback/test assets. Premium production assets must be individually license-checked before inclusion.

## Architecture rule
AI configures and assembles a professionally authored racing template through GameSpec. AI does not redraw the entire game world from ad-hoc geometry for each user request.
