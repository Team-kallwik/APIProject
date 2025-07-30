import 'package:flutter/material.dart';
import 'package:intl/intl.dart';

class BookAppointmentPage extends StatefulWidget {
  @override
  _BookAppointmentPageState createState() => _BookAppointmentPageState();
}

class _BookAppointmentPageState extends State<BookAppointmentPage> {
  final _formKey = GlobalKey<FormState>();

  String? selectedDoctor;
  DateTime? selectedDate;
  TimeOfDay? selectedTime;
  final TextEditingController symptomsController = TextEditingController();

  List<String> doctorList = ['Dr. Sharma', 'Dr. Mehta', 'Dr. Verma'];

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: Text("Book Appointment")),
      body: Padding(
        padding: const EdgeInsets.all(16.0),
        child: Form(
          key: _formKey,
          child: ListView(
            children: [
              DropdownButtonFormField<String>(
                decoration: InputDecoration(
                  labelText: "Select Doctor",
              labelStyle: TextStyle(color: Colors.black,fontSize: 15),
              focusedBorder: OutlineInputBorder(
                borderSide: BorderSide(color: Colors.teal),
              ),
              border: OutlineInputBorder(),
                  prefixIcon: Icon(Icons.person),
                ),
                items: doctorList
                    .map((doc) =>
                    DropdownMenuItem(value: doc, child: Text(doc)))
                    .toList(),
                onChanged: (value) {
                  setState(() {
                    selectedDoctor = value;
                  });
                },
                validator: (value) =>
                value == null ? 'Please select a doctor' : null,
              ),
              SizedBox(height: 16),

              TextFormField(
                readOnly: true,
                decoration: InputDecoration(
                  labelText: 'Select Date',
                  labelStyle: TextStyle(color: Colors.black,fontSize: 15),
                  focusedBorder: OutlineInputBorder(
                    borderSide: BorderSide(color: Colors.teal),
                  ),
                  border: OutlineInputBorder(),
                  prefixIcon: Icon(Icons.calendar_today),
                ),
                onTap: () async {
                  final DateTime? picked = await showDatePicker(
                    context: context,
                    initialDate: DateTime.now(),
                    firstDate: DateTime.now(),
                    lastDate: DateTime(2101),
                  );
                  if (picked != null) {
                    setState(() => selectedDate = picked);
                  }
                },
                controller: TextEditingController(
                    text: selectedDate == null
                        ? ''
                        : DateFormat('yyyy-MM-dd').format(selectedDate!)),
                validator: (value) =>
                value!.isEmpty ? 'Please pick a date' : null,
              ),
              SizedBox(height: 16),

              TextFormField(
                readOnly: true,
                decoration: InputDecoration(
                  labelText: 'Select Time',
                  labelStyle: TextStyle(color: Colors.black,fontSize: 15),
                  focusedBorder: OutlineInputBorder(
                    borderSide: BorderSide(color: Colors.teal),
                  ),
                  border: OutlineInputBorder(),
                  prefixIcon: Icon(Icons.access_time),
                ),
                onTap: () async {
                  final TimeOfDay? picked = await showTimePicker(
                    context: context,
                    initialTime: TimeOfDay.now(),
                  );
                  if (picked != null) {
                    setState(() => selectedTime = picked);
                  }
                },
                controller: TextEditingController(
                    text: selectedTime == null
                        ? ''
                        : selectedTime!.format(context)),
                validator: (value) =>
                value!.isEmpty ? 'Please select time' : null,
              ),
              SizedBox(height: 16),

              TextFormField(
                controller: symptomsController,
                maxLines: 3,
                decoration: InputDecoration(
                  labelText: 'Symptoms/Concerns',
                  labelStyle: TextStyle(color: Colors.black,fontSize: 15),
                  focusedBorder: OutlineInputBorder(
                    borderSide: BorderSide(color: Colors.teal),
                  ),
                  border: OutlineInputBorder(),
                  prefixIcon: Icon(Icons.notes),
                ),
                validator: (value) =>
                value!.isEmpty ? 'Please enter symptoms' : null,
              ),
              SizedBox(height: 24),

              ElevatedButton.icon(
                style: ElevatedButton.styleFrom(
                  backgroundColor: Colors.teal,
                  shape: RoundedRectangleBorder(
                    borderRadius: BorderRadius.circular(12),
                  ),
                ),
                icon: Icon(Icons.check_circle_outline,color: Colors.white),
                label: Text("Confirm Appointment",style: TextStyle(color: Colors.white,fontSize: 15)),
                onPressed: () {
                  if (_formKey.currentState!.validate()) {
                    // Booking logic here
                    showDialog(
                      context: context,
                      builder: (context) => AlertDialog(
                        title: Text("Success"),
                        content: Text(
                            "Appointment booked with $selectedDoctor on ${DateFormat('yyyy-MM-dd').format(selectedDate!)} at ${selectedTime!.format(context)}."),
                        actions: [
                          TextButton(
                            onPressed: () => Navigator.pop(context),
                            child: Text("OK"),
                          )
                        ],
                      ),
                    );
                  }
                },
              )
            ],
          ),
        ),
      ),
    );
  }
}
