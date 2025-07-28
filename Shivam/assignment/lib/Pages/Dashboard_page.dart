import 'package:assignment/Pages/Docter_list_page.dart';
import 'package:assignment/Pages/Health_record_page.dart';
import 'package:flutter/material.dart';

class DashboardPage extends StatelessWidget {
  final List<String> appointments = [
    "Dr. Sharma - 29 July",
    "Dr. Mehta - 31 July",
    "Dr. Verma - 2 Aug",
  ];

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: Text("Dashboard")),
      bottomNavigationBar: BottomNavigationBar(
        items: const [
          BottomNavigationBarItem(icon: Icon(Icons.home), label: 'Home'),
          BottomNavigationBarItem(
            icon: Icon(Icons.calendar_today),
            label: 'Appointments',
          ),
          BottomNavigationBarItem(icon: Icon(Icons.person), label: 'Profile'),
        ],
        currentIndex: 0,
        onTap: (index) {},
      ),
      body: SingleChildScrollView(
        padding: EdgeInsets.all(16),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text(
              "Hello, User!",
              style: TextStyle(fontSize: 24, fontWeight: FontWeight.bold),
            ),
            SizedBox(height: 20),
            Text(
              "Upcoming Appointments",
              style: TextStyle(fontSize: 18, fontWeight: FontWeight.w600),
            ),
            SizedBox(height: 10),
            Container(
              height: 100,
              child: ListView.separated(
                scrollDirection: Axis.horizontal,
                itemCount: appointments.length,
                separatorBuilder: (_, __) => SizedBox(width: 10),
                itemBuilder: (context, index) {
                  return Card(
                    elevation: 3,
                    child: Padding(
                      padding: const EdgeInsets.all(12.0),
                      child: Center(child: Text(appointments[index])),
                    ),
                  );
                },
              ),
            ),
            SizedBox(height: 30),
            Text(
              "Quick Actions",
              style: TextStyle(fontSize: 18, fontWeight: FontWeight.w600),
            ),
            SizedBox(height: 10),
            GridView.count(
              crossAxisCount: 2,
              shrinkWrap: true,
              physics: NeverScrollableScrollPhysics(),
              crossAxisSpacing: 10,
              mainAxisSpacing: 10,
              children: [
                ActionTile(
                  icon: Icons.add_circle,
                  label: 'Book Appointment',
                  onTap: () {},
                ),
                ActionTile(
                  icon: Icons.folder,
                  label: 'Health Records',
                  onTap: () => Navigator.push(
                    context,
                    MaterialPageRoute(builder: (_) => HealthRecordsPage()),
                  ),
                ),
                ActionTile(
                  icon: Icons.medical_services,
                  label: 'View Doctors',
                  onTap: () => Navigator.push(
                    context,
                    MaterialPageRoute(builder: (_) => DoctorsListPage()),
                  ),
                ),
                ActionTile(
                  icon: Icons.settings,
                  label: 'Settings',
                  onTap: () {},
                ),
              ],
            ),
          ],
        ),
      ),
    );
  }
}

class ActionTile extends StatelessWidget {
  final IconData icon;
  final String label;
  final VoidCallback onTap;

  ActionTile({required this.icon, required this.label, required this.onTap});

  @override
  Widget build(BuildContext context) {
    return GestureDetector(
      onTap: onTap,
      child: Card(
        elevation: 4,
        shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(10)),
        child: Center(
          child: Column(
            mainAxisSize: MainAxisSize.min,
            children: [
              Icon(icon, size: 40, color: Colors.green),
              SizedBox(height: 10),
              Text(label, textAlign: TextAlign.center),
            ],
          ),
        ),
      ),
    );
  }
}
