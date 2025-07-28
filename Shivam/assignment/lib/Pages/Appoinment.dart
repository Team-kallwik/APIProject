import 'package:flutter/material.dart';

class AppointmentsPage extends StatelessWidget {
  final List<Map<String, String>> appointments = [
    {
      'doctor': 'Dr. Priya Sharma',
      'date': '30 July 2025',
      'time': '10:00 AM',
      'status': 'Confirmed',
    },
    {
      'doctor': 'Dr. Rohan Mehta',
      'date': '28 July 2025',
      'time': '03:00 PM',
      'status': 'Cancelled',
    },
    {
      'doctor': 'Dr. Anjali Verma',
      'date': '25 July 2025',
      'time': '09:00 AM',
      'status': 'Confirmed',
    },
  ];

  Color getStatusColor(String status) {
    switch (status) {
      case 'Confirmed':
        return Colors.green;
      case 'Cancelled':
        return Colors.red;
      default:
        return Colors.grey;
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: Text("Appointments")),
      body: ListView.builder(
        padding: const EdgeInsets.all(16),
        itemCount: appointments.length,
        itemBuilder: (context, index) {
          final appt = appointments[index];
          return Card(
            elevation: 3,
            margin: const EdgeInsets.symmetric(vertical: 8),
            child: ListTile(
              leading: Icon(Icons.medical_services, color: Colors.teal),
              title: Text(
                appt['doctor']!,
                style: TextStyle(fontWeight: FontWeight.bold),
              ),
              subtitle: Text("${appt['date']} at ${appt['time']}"),
              trailing: Text(
                appt['status']!,
                style: TextStyle(
                  color: getStatusColor(appt['status']!),
                  fontWeight: FontWeight.bold,
                ),
              ),
            ),
          );
        },
      ),
    );
  }
}
