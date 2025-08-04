import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:intl/intl.dart';
import '../Controller/appointment_controller.dart';
class BookAppointmentPage extends StatelessWidget {
  final AppointmentController controller = Get.put(AppointmentController());
  final _formKey = GlobalKey<FormState>();
  final TextEditingController symptomsController = TextEditingController();

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        leading: BackButton(color: Colors.white,),
        title: Text('Book Appointment',style: TextStyle(color: Colors.white,fontWeight: FontWeight.bold),),
        backgroundColor: Colors.teal,
      ),
      body: SingleChildScrollView(
        padding: EdgeInsets.all(16),
        child: Form(
          key: _formKey,
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              // Doctor Selection
              Text("Select Doctor", style: TextStyle(fontWeight: FontWeight.bold)),
              SizedBox(height: 8),
              Obx(() => DropdownButtonFormField<String>(
                value: controller.selectedDoctor.value,
                decoration: InputDecoration(
                  prefixIcon: Icon(Icons.person),
                  border: OutlineInputBorder(),
                  focusedBorder: OutlineInputBorder(
                    borderSide: BorderSide(color: Colors.teal),
                  ),
                ),
                items: ['Dr. Sharma', 'Dr. Mehta', 'Dr. Verma']
                    .map((doc) => DropdownMenuItem(
                  value: doc,
                  child: Text(doc),
                ))
                    .toList(),
                onChanged: (value) => controller.selectedDoctor.value = value,
                validator: (value) => value == null ? 'Please select a doctor' : null,
              )),
              SizedBox(height: 16),

              // Date Picker
              Text("Select Date", style: TextStyle(fontWeight: FontWeight.bold)),
              SizedBox(height: 8),
              Obx(() {
                return TextFormField(
                  readOnly: true,
                  controller: TextEditingController(
                    text: controller.selectedDate.value == null
                        ? ''
                        : DateFormat('yyyy-MM-dd').format(controller.selectedDate.value!),
                  ),
                  decoration: InputDecoration(
                    prefixIcon: Icon(Icons.calendar_today),
                    border: OutlineInputBorder(),
                    focusedBorder: OutlineInputBorder(
                      borderSide: BorderSide(color: Colors.teal),
                    ),
                  ),
                  onTap: () async {
                    final picked = await showDatePicker(
                      context: context,
                      initialDate: DateTime.now(),
                      firstDate: DateTime.now(),
                      lastDate: DateTime(2101),
                    );
                    if (picked != null) controller.selectedDate.value = picked;
                  },
                  validator: (value) => value!.isEmpty ? 'Please select a date' : null,
                );
              }),
              SizedBox(height: 16),

              // Time Picker
              Text("Select Time", style: TextStyle(fontWeight: FontWeight.bold)),
              SizedBox(height: 8),
              Obx(() {
                return TextFormField(
                  readOnly: true,
                  controller: TextEditingController(
                    text: controller.selectedTime.value == null
                        ? ''
                        : controller.selectedTime.value!.format(context),
                  ),
                  decoration: InputDecoration(
                    prefixIcon: Icon(Icons.access_time),
                    border: OutlineInputBorder(),
                    focusedBorder: OutlineInputBorder(
                      borderSide: BorderSide(color: Colors.teal),
                    ),
                  ),
                  onTap: () async {
                    final picked = await showTimePicker(
                      context: context,
                      initialTime: TimeOfDay.now(),
                    );
                    if (picked != null) controller.selectedTime.value = picked;
                  },
                  validator: (value) => value!.isEmpty ? 'Please select a time' : null,
                );
              }),
              SizedBox(height: 16),

              // Symptoms
              Text("Symptoms/Concerns", style: TextStyle(fontWeight: FontWeight.bold)),
              SizedBox(height: 8),
              TextFormField(
                controller: symptomsController,
                maxLines: 3,
                decoration: InputDecoration(
                  prefixIcon: Icon(Icons.notes),
                  border: OutlineInputBorder(),
                  focusedBorder: OutlineInputBorder(
                    borderSide: BorderSide(color: Colors.teal),
                  ),
                ),
                onChanged: controller.updateSymptoms,
                validator: (value) => value!.isEmpty ? 'Please enter your symptoms' : null,
              ),
              SizedBox(height: 24),
              // Confirm Button
              Center(
                child: ElevatedButton.icon(
                  style: ElevatedButton.styleFrom(
                    backgroundColor: Colors.teal,
                    shape: RoundedRectangleBorder(
                      borderRadius: BorderRadius.circular(12),
                    ),
                    padding: EdgeInsets.symmetric(horizontal: 24, vertical: 12),
                  ),
                  icon: Icon(Icons.check_circle_outline, color: Colors.white),
                  label: Text("Confirm Appointment", style: TextStyle(color: Colors.white)),
                  onPressed: () {
                    if (_formKey.currentState!.validate()) {
                      Get.dialog(
                        AlertDialog(
                          title: Text("Success"),
                          content: Text(
                            "Appointment booked with ${controller.selectedDoctor.value} on "
                                "${DateFormat('yyyy-MM-dd').format(controller.selectedDate.value!)} at "
                                "${controller.selectedTime.value!.format(context)}.\n\n"
                                "Symptoms: ${controller.symptoms.value}",
                          ),
                          actions: [
                            TextButton(
                              onPressed: () => Get.back(),
                              child: Text("OK"),
                            ),
                          ],
                        ),
                      );
                    }
                  },
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }
}
