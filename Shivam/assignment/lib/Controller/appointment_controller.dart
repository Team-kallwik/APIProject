import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:intl/intl.dart';

class Appointment {
  String doctor;
  DateTime date;
  TimeOfDay time;
  String symptoms;
  String status;

  Appointment({
    required this.doctor,
    required this.date,
    required this.time,
    required this.symptoms,
    this.status = "Confirmed",
  });

  String get dateString => DateFormat('yyyy-MM-dd').format(date);
  String get formattedTime => time.format(Get.context!);
}

class AppointmentController extends GetxController {
  RxList<Appointment> appointments = <Appointment>[].obs;

  var selectedDoctor = ''.obs;
  var selectedDate = Rxn<DateTime>();
  var selectedTime = Rxn<TimeOfDay>();
  var symptoms = ''.obs;

  void updateSymptoms(String value) {
    symptoms.value = value;
  }

  void confirmAppointment() {
    final newAppointment = Appointment(
      doctor: selectedDoctor.value,
      date: selectedDate.value!,
      time: selectedTime.value!,
      symptoms: symptoms.value,
      status: "Confirmed",
    );

    appointments.add(newAppointment);
  }

  void deleteAppointment(Appointment appt) {
    appointments.remove(appt);
  }

  void editAppointment(Appointment oldAppt, Appointment updatedAppt) {
    final index = appointments.indexOf(oldAppt);
    if (index != -1) {
      appointments[index] = updatedAppt;
    }
  }

  void loadAppointmentForEdit(Appointment appt) {
    selectedDoctor.value = appt.doctor;
    selectedDate.value = appt.date;
    selectedTime.value = appt.time;
    symptoms.value = appt.symptoms;
  }

  void resetForm() {
    selectedDoctor.value = '';
    selectedDate.value = null;
    selectedTime.value = null;
    symptoms.value = '';
  }
}
