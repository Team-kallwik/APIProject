import 'package:flutter/material.dart';

class DoctorsListPage extends StatefulWidget {
  @override
  _DoctorsListPageState createState() => _DoctorsListPageState();
}

class _DoctorsListPageState extends State<DoctorsListPage> {
  final List<Map<String, dynamic>> doctors = [
    {
      'name': 'Dr. Kashish Rithe',
      'specialty': 'Cardiologist',
      'rating': 4.8,
      'image': 'https://via.placeholder.com/150',
    },
    {
      'name': 'Dr. Yaju Patel',
      'specialty': 'psychiatrist ',
      'rating': 5.0,
      'image': 'https://via.placeholder.com/150',
    },
    {
      'name': 'Dr. Abhishek Raghuvanshi',
      'specialty': 'Pediatrician',
      'rating': 4.9,
      'image': 'https://via.placeholder.com/150',
    },
    {
      'name': 'Dr. Priyansh Pal',
      'specialty': 'Neurologist',
      'rating': 4.9,
      'image': 'https://via.placeholder.com/150',
    },
    {
      'name': 'Dr. Vaishnavi Bajpai',
      'specialty': 'Veterinary Ophthalmologist',
      'rating': 4.8,
      'image': 'https://via.placeholder.com/150',
    },
    {
      'name': 'Dr. Vikas Tiwari',
      'specialty': 'Dermatologist',
      'rating': 4.6,
      'image': 'https://via.placeholder.com/150',
    },
    {
      'name': 'Dr.Dhananjay singh Panwar',
      'specialty': 'Dentist',
      'rating': 4.8,
      'image': 'https://via.placeholder.com/150',
    },
    {
      'name': 'Dr.Abhishek Sharma',
      'specialty': 'Psychiatrist',
      'rating': 4.9,
      'image': 'https://via.placeholder.com/150',
    },

  ];

  String searchQuery = '';

  @override
  Widget build(BuildContext context) {
    final filteredDoctors = doctors.where((doc) {
      return doc['name'].toLowerCase().contains(searchQuery.toLowerCase()) ||
          doc['specialty'].toLowerCase().contains(searchQuery.toLowerCase());
    }).toList();

    return Scaffold(
      appBar: AppBar(title: Text("Doctors")),
      body: Column(
        children: [
          Padding(
            padding: const EdgeInsets.all(16.0),
            child: TextField(
              decoration: InputDecoration(
                hintText: 'Search doctor or specialty...',
                prefixIcon: Icon(Icons.search),
                border: OutlineInputBorder(
                  borderRadius: BorderRadius.circular(12),
                ),
              ),
              onChanged: (value) {
                setState(() {
                  searchQuery = value;
                });
              },
            ),
          ),
          Expanded(
            child: ListView.builder(
              itemCount: filteredDoctors.length,
              itemBuilder: (context, index) {
                final doctor = filteredDoctors[index];
                return Card(
                  margin: const EdgeInsets.symmetric(
                    horizontal: 16,
                    vertical: 8,
                  ),
                  child: ListTile(
                    leading: CircleAvatar(
                      backgroundImage: NetworkImage(doctor['image']),
                      radius: 28,
                    ),
                    title: Text(
                      doctor['name'],
                      style: TextStyle(fontWeight: FontWeight.bold),
                    ),
                    subtitle: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Text(doctor['specialty']),
                        Row(
                          children: [
                            Icon(Icons.star, color: Colors.amber, size: 16),
                            SizedBox(width: 4),
                            Text('${doctor['rating']}'),
                          ],
                        ),
                      ],
                    ),
                    trailing: ElevatedButton(
                      onPressed: () {},
                      child: Text('Book Now',style: TextStyle(color: Colors.white),),
                      style: ElevatedButton.styleFrom(
                        backgroundColor: Colors.teal,
                      ),
                    ),
                  ),
                );
              },
            ),
          ),
        ],
      ),
    );
  }
}
