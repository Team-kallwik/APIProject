import 'package:flutter/material.dart';

class HealthRecordsPage extends StatefulWidget {
  @override
  _HealthRecordsPageState createState() => _HealthRecordsPageState();
}

class _HealthRecordsPageState extends State<HealthRecordsPage>
    with SingleTickerProviderStateMixin {
  late TabController _tabController;

  @override
  void initState() {
    super.initState();
    _tabController = TabController(length: 3, vsync: this);
  }

  @override
  void dispose() {
    _tabController.dispose();
    super.dispose();
  }

  Widget buildRecordList(String type) {
    return ListView.builder(
      padding: const EdgeInsets.all(16),
      itemCount: 3,
      itemBuilder: (context, index) {
        return Card(
          elevation: 2,
          margin: const EdgeInsets.symmetric(vertical: 8),
          child: ListTile(
            leading: Icon(
              type == 'Reports'
                  ? Icons.picture_as_pdf
                  : type == 'Prescriptions'
                  ? Icons.description
                  : Icons.bloodtype,
              color: Colors.teal,
            ),
            title: Text('$type Record ${index + 1}'),
            subtitle: Text('Added on 27 July 2025'),
            trailing: Icon(Icons.arrow_forward_ios, size: 16),
            onTap: () {},
          ),
        );
      },
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text("Health Records"),
        bottom: TabBar(
          controller: _tabController,
          tabs: [
            Tab(text: 'Reports'),
            Tab(text: 'Prescriptions'),
            Tab(text: 'Lab Results'),
          ],
        ),
      ),
      body: TabBarView(
        controller: _tabController,
        children: [
          buildRecordList('Reports'),
          buildRecordList('Prescriptions'),
          buildRecordList('Lab Results'),
        ],
      ),
    );
  }
}
