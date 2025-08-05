import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:intl/intl.dart';

import '../Controller/appointment_controller.dart';
import '../data/doctor_data.dart'; // ✅ Import doctor list

class BookAppointmentPage extends StatelessWidget {
  final bool isEditing;
  final Appointment? existingAppointment;

  BookAppointmentPage({
    this.isEditing = false,
    this.existingAppointment,
    Key? key,
  }) : super(key: key);
  final AppointmentController controller = Get.put(AppointmentController());
  final _formKey = GlobalKey<FormState>();
  final TextEditingController symptomsController = TextEditingController();

  @override
  Widget build(BuildContext context) {
    if (isEditing && existingAppointment != null) {
      controller.loadAppointmentForEdit(existingAppointment!);
    }
    return Scaffold(
      appBar: AppBar(
        leading: const BackButton(color: Colors.white),
        title: const Text(
          'Book Appointment',
          style: TextStyle(color: Colors.white, fontWeight: FontWeight.bold),
        ),
        backgroundColor: Colors.teal,
      ),
      body: SingleChildScrollView(
        padding: const EdgeInsets.all(16),
        child: Form(
          key: _formKey,
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              // Doctor Selection
              const Text("Select Doctor", style: TextStyle(fontWeight: FontWeight.bold)),
              const SizedBox(height: 8),
              Obx(() => DropdownButtonFormField<String>(
                value: controller.selectedDoctor.value.isEmpty
                    ? null
                    : controller.selectedDoctor.value,
                decoration: const InputDecoration(
                  prefixIcon: Icon(Icons.person),
                  border: OutlineInputBorder(),
                  focusedBorder: OutlineInputBorder(
                    borderSide: BorderSide(color: Colors.teal),
                  ),
                ),
                items: doctorNames.map((doc) {
                  return DropdownMenuItem(
                    value: doc,
                    child: Text(doc),
                  );
                }).toList(),
                onChanged: (value) {
                  controller.selectedDoctor.value = value ?? '';
                },
                validator: (value) => value == null || value.isEmpty ? 'Please select a doctor' : null,
              )),
              const SizedBox(height: 16),

              // Date Picker
              const Text("Select Date", style: TextStyle(fontWeight: FontWeight.bold)),
              const SizedBox(height: 8),
              Obx(() {
                return TextFormField(
                  readOnly: true,
                  controller: TextEditingController(
                    text: controller.selectedDate.value == null
                        ? ''
                        : DateFormat('yyyy-MM-dd').format(controller.selectedDate.value!),
                  ),
                  decoration: const InputDecoration(
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
              const SizedBox(height: 16),

              // Time Picker
              const Text("Select Time", style: TextStyle(fontWeight: FontWeight.bold)),
              const SizedBox(height: 8),
              Obx(() {
                return TextFormField(
                  readOnly: true,
                  controller: TextEditingController(
                    text: controller.selectedTime.value == null
                        ? ''
                        : controller.selectedTime.value!.format(context),
                  ),
                  decoration: const InputDecoration(
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
              const SizedBox(height: 16),

              // Symptoms
              const Text("Symptoms/Concerns", style: TextStyle(fontWeight: FontWeight.bold)),
              const SizedBox(height: 8),
              TextFormField(
                controller: symptomsController,
                maxLines: 3,
                decoration: const InputDecoration(
                  prefixIcon: Icon(Icons.notes),
                  border: OutlineInputBorder(),
                  focusedBorder: OutlineInputBorder(
                    borderSide: BorderSide(color: Colors.teal),
                  ),
                ),
                onChanged: controller.updateSymptoms,
                validator: (value) => value!.isEmpty ? 'Please enter your symptoms' : null,
              ),
              const SizedBox(height: 24),

              // Confirm Button
              Center(
                child: ElevatedButton.icon(
                  style: ElevatedButton.styleFrom(
                    backgroundColor: Colors.teal,
                    shape: RoundedRectangleBorder(
                      borderRadius: BorderRadius.circular(12),
                    ),
                    padding: const EdgeInsets.symmetric(horizontal: 24, vertical: 12),
                  ),
                  icon: const Icon(Icons.check_circle_outline, color: Colors.white),
                  label: const Text("Confirm Appointment", style: TextStyle(color: Colors.white)),
                    onPressed: () {
                      if (_formKey.currentState!.validate()) {
                        if (isEditing && existingAppointment != null) {
                          final updatedAppointment = Appointment(
                            doctor: controller.selectedDoctor.value,
                            date: controller.selectedDate.value!,
                            time: controller.selectedTime.value!,
                            symptoms: controller.symptoms.value,
                            status: existingAppointment!.status, // preserve status
                          );
                          controller.editAppointment(existingAppointment!, updatedAppointment);
                        } else {
                          controller.confirmAppointment();
                        }

                        Get.dialog(
                          AlertDialog(
                            title: Text("Success"),
                            content: Text(
                              "Appointment ${isEditing ? 'updated' : 'booked'} with ${controller.selectedDoctor.value} "
                                  "on ${DateFormat('yyyy-MM-dd').format(controller.selectedDate.value!)} at "
                                  "${controller.selectedTime.value!.format(context)}.\n\nSymptoms: ${controller.symptoms.value}",
                            ),
                            actions: [
                              TextButton(
                                onPressed: () {
                                  Get.back(); // close dialog
                                  Get.back(); // go back to appointments page
                                  controller.resetForm();
                                  symptomsController.clear();
                                },
                                child: const Text("OK"),
                              ),
                            ],
                          ),
                        );
                      }
                    }
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }
}
