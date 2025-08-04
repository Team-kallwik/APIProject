import 'package:get/get.dart';
import 'package:flutter/material.dart';

class AppointmentController extends GetxController {
  var selectedDoctor = RxnString();      // Nullable String
  var selectedDate = Rxn<DateTime>();    // Nullable DateTime
  var selectedTime = Rxn<TimeOfDay>();   // Nullable TimeOfDay
  var symptoms = ''.obs;

  void updateSymptoms(String value) {
    symptoms.value = value;
  }
}
