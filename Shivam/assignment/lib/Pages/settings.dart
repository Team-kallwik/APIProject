import "package:flutter/material.dart";

import "Profile_page.dart";

class Setting extends StatefulWidget {
  const Setting({super.key});

  @override
  State<Setting> createState() => _SettingState();
}

class _SettingState extends State<Setting> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: SafeArea(
        child: Column(
          children: [
            Padding(
              padding: const EdgeInsets.all(16.0),
              child: Text(
                "Settings",
                style: TextStyle(fontSize: 24, fontWeight: FontWeight.bold),
              ),
            ),
            ListTile(
              leading: Icon(Icons.account_circle),
              title: Text("Profile"),
              onTap: () {
               Navigator.push(context, MaterialPageRoute(builder: (context)=> ProfilePage()));
              },
            ),
            ListTile(
              leading: Icon(Icons.notifications),
              title: Text("Notifications"),
              onTap: () {
                // Navigate to notifications settings
              },
            ),
            ListTile(
              leading: Icon(Icons.security),
              title: Text("Security"),
              onTap: () {
                // Navigate to security settings
              },
            ),
          ],
        ),
      ),
    );
  }
}
