import 'package:assignment/Pages/login.dart';
import 'package:flutter/material.dart';

class ProfilePage extends StatefulWidget {
  @override
  State<ProfilePage> createState() => _ProfilePageState();
}

class _ProfilePageState extends State<ProfilePage> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text("My Profile"),
        centerTitle: true,
      ),
      body: SingleChildScrollView(
        padding: const EdgeInsets.all(16),
        child: Column(
          children: [
            Stack(
              alignment: Alignment.bottomRight,
              children: [
                CircleAvatar(
                  radius: 60,
                  backgroundImage: AssetImage('assets/images/background.jpg'), // Replace with user image
                ),
                IconButton(
                  icon: const Icon(Icons.edit, color: Colors.teal),
                  onPressed: () {
                    // Add edit photo logic
                  },
                ),
              ],
            ),
            const SizedBox(height: 20),

            // Name
            const Text(
              'Shivam Gour',
              style: TextStyle(fontSize: 22, fontWeight: FontWeight.bold,color: Colors.black),
            ),
            const SizedBox(height: 4),
            const Text(
              'shivamsingh@gmail.com',
              style: TextStyle(color: Colors.black45),
            ),

            const SizedBox(height: 20),

            // Basic Info
            ProfileTile(title: 'Phone', value: '+91 9876543210'),
            ProfileTile(title: 'Gender', value: 'Male'),
            ProfileTile(title: 'Age', value: '21'),

            const SizedBox(height: 20),

            // Health Info
            const Align(
              alignment: Alignment.centerLeft,
              child: Text(
                'Health Information',
                style: TextStyle(fontSize: 18, fontWeight: FontWeight.bold),
              ),
            ),
            const SizedBox(height: 10),
            ProfileTile(title: 'Blood Group', value: 'O+'),
            ProfileTile(title: 'Height', value: '183 cm'),
            ProfileTile(title: 'Weight', value: '80 kg'),
          ],
        ),
      ),
    );
  }
}

class ProfileTile extends StatelessWidget {
  final String title;
  final String value;

  const ProfileTile({
    required this.title,
    required this.value,
    Key? key,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return ListTile(
      contentPadding: const EdgeInsets.symmetric(vertical: 4, horizontal: 0),
      title: Text(
        title,
        style: const TextStyle(color: Colors.black45),
      ),
      subtitle: Text(
        value,
        style: const TextStyle(fontSize: 16),
      ),
    );
  }
}
