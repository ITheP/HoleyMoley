Holey Moley
© 2010-2021 MisterB@ITheP

Sticks visual windows over your desktop + a few extra things!

Release change history...

v1.3 Draft - 12/10/2021
- First upload of private version to GitHub

v1.6 Test - 08/11/2021
- First upload of public test version to GitHub (allow for checking some basics on other pc's)

v1.7 Test - 10/11/2021
- Upgrade to .net 6
- Remove dependency on VisualBasic.PowerPacks (was used for rectangle control)
- Windows min/max/title bar tweaks
- Logo changes

v1.7.2 Test - 10/12/2021
- Ready to go out into the wild for the first time test

v1.7.1 Alpha - 17/10/2022
- First publish of a reasonably working version

ToDo:
- Make sure all settings are saving/restoring properly, including what is enabled/disabled
- User defined highlighting colours (at the moment, not colour blind friendly)
- Get application path etc. from hwnd
- ...only highlight for certain applications
- Change depth of border for certain window types? Maybe base on client area rather than window area? (e.g. Edge etc. have a shadow around them that make their window area larger than what a border actually would be)
- Re-work versioning and About information
- Auto-update application via GitHub
- Upgrade framework
- Be nice to re-write in a newer framework / .Net Core6/WinUI3?
- Minimise to task bar, not just small window
- Redo the logo, don't like it any more
- Remote desktop window not highlighting + on resize, it can play silly buggers a bit - might need to check out what happens on screen resolution changes
- Fancy effects? Like fading out the highlighting - more like a glow? Could punch a hole in the middle of the windows then (transparent windows) so block colour doesnt flash up behind windows during certain events.
