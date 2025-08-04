import 'package:assignment/Pages/login.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'Profile_page.dart';

class Setting extends StatefulWidget {
  const Setting({super.key});

  @override
  State<Setting> createState() => _SettingState();
}

class _SettingState extends State<Setting> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text("Settings"),
        leading: const BackButton(),
        centerTitle: true,
      ),
      body: SafeArea(
        child: ListView(
          padding: const EdgeInsets.all(16),
          children: [
            const SizedBox(height: 10),
            SettingItem(
              icon: Icons.account_circle,
              title: "Profile",
              onTap: () {
                Get.to(()=> ProfilePage());
                },
            ),
            SettingItem(
              icon: Icons.notifications,
              title: "Notifications",
              onTap: () {
                // Notifications settings
              },
            ),
            SettingItem(
              icon: Icons.lock,
              title: "Security",
              onTap: () {
                // Security settings
              },
            ),
            SettingItem(
              icon: Icons.privacy_tip,
              title: "Privacy Policy",
              onTap: () {
                // Open Privacy Policy
              },
            ),
            SettingItem(
              icon: Icons.help_outline,
              title: "Help & Support",
              onTap: () {
                // Open Help & Support
              },
            ),
            SettingItem(
              icon: Icons.brightness_6,
              title: "App Theme",
              onTap: () {
                // Theme toggle logic
              },
            ),
            SettingItem(
              icon: Icons.logout,
              title: "Logout",
              onTap: () {
                Get.to(()=> LoginPage());
              },
              iconColor: Colors.red,
              textColor: Colors.red,
            ),
          ],
        ),
      ),
    );
  }
}

class SettingItem extends StatelessWidget {
  final IconData icon;
  final String title;
  final VoidCallback onTap;
  final Color? iconColor;
  final Color? textColor;

  const SettingItem({
    Key? key,
    required this.icon,
    required this.title,
    required this.onTap,
    this.iconColor,
    this.textColor,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Card(
      margin: const EdgeInsets.symmetric(vertical: 8),
      shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12)),
      elevation: 2,
      child: ListTile(
        leading: Icon(icon, color: iconColor ?? Colors.teal),
        title: Text(
          title,
          style: TextStyle(fontSize: 16, color: textColor ?? Colors.black87),
        ),
        trailing: const Icon(Icons.arrow_forward_ios, size: 16),
        onTap: onTap,
      ),
    );
  }
}
