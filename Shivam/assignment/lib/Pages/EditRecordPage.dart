import 'package:flutter/material.dart';

class EditRecordPage extends StatefulWidget {
  final String doctorName;
  final String hospitalName;
  final String notes;

  const EditRecordPage({
    super.key,
    required this.doctorName,
    required this.hospitalName,
    required this.notes,
  });

  @override
  State<EditRecordPage> createState() => _EditRecordPageState();
}

class _EditRecordPageState extends State<EditRecordPage> {
  late TextEditingController _doctorController;
  late TextEditingController _hospitalController;
  late TextEditingController _notesController;

  @override
  void initState() {
    super.initState();
    _doctorController = TextEditingController(text: widget.doctorName);
    _hospitalController = TextEditingController(text: widget.hospitalName);
    _notesController = TextEditingController(text: widget.notes);
  }

  @override
  void dispose() {
    _doctorController.dispose();
    _hospitalController.dispose();
    _notesController.dispose();
    super.dispose();
  }

  void _saveChanges() {
    Navigator.pop(context, {
      'doctor': _doctorController.text,
      'hospital': _hospitalController.text,
      'notes': _notesController.text,
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text("Edit Record", style: TextStyle(color: Colors.white)),
        backgroundColor: Colors.teal,
        leading: const BackButton(color: Colors.white),
        actions: [
          IconButton(icon: const Icon(Icons.save), onPressed: _saveChanges),
        ],
      ),
      body: Padding(
        padding: const EdgeInsets.all(16),
        child: Column(
          children: [
            TextFormField(
              controller: _doctorController,
              decoration: const InputDecoration(
                labelStyle: TextStyle(color: Colors.black),
                labelText: 'Doctor Name',
                focusedBorder: OutlineInputBorder(
                  borderSide: BorderSide(color: Colors.teal)
                ),
                border: OutlineInputBorder(),
              ),
            ),
            const SizedBox(height: 16),
            TextFormField(
              controller: _hospitalController,
              decoration: const InputDecoration(
                labelStyle: TextStyle(color: Colors.black),
                labelText: 'Hospital Name',
                focusedBorder: OutlineInputBorder(
                    borderSide: BorderSide(color: Colors.teal)
                ),
                border: OutlineInputBorder(),
              ),
            ),
            const SizedBox(height: 16),
            TextFormField(
              controller: _notesController,
              maxLines: 5,
              decoration: const InputDecoration(
                labelStyle: TextStyle(color: Colors.black),
                labelText: 'Doctor\'s Notes',
                alignLabelWithHint: true,
                focusedBorder: OutlineInputBorder(
                    borderSide: BorderSide(color: Colors.teal)
                ),
                border: OutlineInputBorder(),
              ),
            ),
          ],
        ),
      ),
    );
  }
}
