import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:intl/intl.dart';
import 'package:table_calendar/table_calendar.dart';
import '../Controller/appointment_controller.dart';
import 'BookAppoinmentPage.dart';

class AppointmentsPage extends StatefulWidget {
  @override
  _AppointmentsPageState createState() => _AppointmentsPageState();
}

class _AppointmentsPageState extends State<AppointmentsPage> {
  final AppointmentController controller = Get.find();

  DateTime _focusedDay = DateTime.now();
  DateTime? _selectedDay;
  CalendarFormat _calendarFormat = CalendarFormat.week;
  String _searchQuery = '';
  String _statusFilter = 'All';

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

  List<Appointment> getAppointmentsForDay(DateTime day) {
    String formatted = DateFormat('yyyy-MM-dd').format(day);
    return controller.appointments.where((appt) {
      final matchesDate = appt.dateString == formatted;
      final matchesDoctor =
      appt.doctor.toLowerCase().contains(_searchQuery.toLowerCase());
      final matchesStatus =
          _statusFilter == 'All' || appt.status == _statusFilter;
      return matchesDate && matchesDoctor && matchesStatus;
    }).toList();
  }

  @override
  Widget build(BuildContext context) {
    final selectedDay = _selectedDay ?? _focusedDay;
    return Scaffold(
      appBar: AppBar(
        backgroundColor: Colors.teal,
        leading: BackButton(color: Colors.white,
        onPressed: (){
          Get.back();
        },
        ),
        title: const Text(
          "Appointments",
          style: TextStyle(color: Colors.white, fontWeight: FontWeight.bold),
        ),
        actions: [
          IconButton(
            icon: const Icon(Icons.calendar_view_month, color: Colors.white),
            onPressed: () {
              setState(() {
                _calendarFormat = _calendarFormat == CalendarFormat.week
                    ? CalendarFormat.month
                    : CalendarFormat.week;
              });
            },
            tooltip: 'Toggle Week/Month View',
          ),
        ],
      ),
      body: SingleChildScrollView(
    child: Column(
    crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        TableCalendar(
          firstDay: DateTime.utc(2020),
          lastDay: DateTime.utc(2030),
          focusedDay: _focusedDay,
          calendarFormat: _calendarFormat,
          selectedDayPredicate: (day) => isSameDay(_selectedDay, day),
          onDaySelected: (selected, focused) {
            setState(() {
              _selectedDay = selected;
              _focusedDay = focused;
            });
          },
          onFormatChanged: (format) {
            setState(() {
              _calendarFormat = format;
            });
          },
          calendarStyle: const CalendarStyle(
            todayDecoration: BoxDecoration(
              color: Colors.tealAccent,
              shape: BoxShape.circle,
            ),
            selectedDecoration: BoxDecoration(
              color: Colors.teal,
              shape: BoxShape.circle,
            ),
          ),
        ),
        Padding(
          padding: const EdgeInsets.all(8.0),
          child: Column(
            children: [
              TextField(
                decoration: InputDecoration(
                  hintText: 'Search by doctor name...',
                  prefixIcon: const Icon(Icons.search),
                  border: OutlineInputBorder(
                      borderRadius: BorderRadius.circular(12)),
                ),
                onChanged: (value) {
                  setState(() {
                    _searchQuery = value;
                  });
                },
              ),
              const SizedBox(height: 10),
              Row(
                mainAxisAlignment: MainAxisAlignment.center,
                children: [
                  const Text("Status: "),
                  DropdownButton<String>(
                    value: _statusFilter,
                    items: ['All', 'Confirmed', 'Cancelled']
                        .map((status) => DropdownMenuItem(
                      value: status,
                      child: Text(status),
                    ))
                        .toList(),
                    onChanged: (value) {
                      if (value != null) {
                        setState(() {
                          _statusFilter = value;
                        });
                      }
                    },
                  ),
                ],
              ),
            ],
          ),
        ),
        Padding(
          padding: const EdgeInsets.symmetric(horizontal: 16.0),
          child: Row(
            children: [
              Obx(() {
                final todaysAppointments = getAppointmentsForDay(selectedDay);
                return Text(
                  "Appointments: ${todaysAppointments.length}",
                  style: const TextStyle(fontWeight: FontWeight.bold),
                );
              }),
            ],
          ),
        ),
        const SizedBox(height: 10),
        Obx(() {
          final todaysAppointments = getAppointmentsForDay(selectedDay);
          return todaysAppointments.isEmpty
              ? const Center(child: Text("No appointments for this day."))
              : ListView.builder(
            itemCount: todaysAppointments.length,
            shrinkWrap: true,
            physics: NeverScrollableScrollPhysics(),
            padding: const EdgeInsets.symmetric(horizontal: 16),
            itemBuilder: (context, index) {
              final appt = todaysAppointments[index];
              return Card(
                elevation: 3,
                margin: const EdgeInsets.symmetric(vertical: 6),
                child: ListTile(
                  leading: const Icon(Icons.medical_services,
                      color: Colors.teal),
                  title: Text(
                    appt.doctor,
                    style: const TextStyle(fontWeight: FontWeight.bold),
                  ),
                  subtitle: Text(
                    "${DateFormat('dd MMM yyyy').format(appt.date)} at ${appt.formattedTime}",
                  ),
                  trailing: Row(
                    mainAxisSize: MainAxisSize.min,
                    children: [
                      IconButton(
                        icon: const Icon(Icons.edit, color: Colors.blue),
                        onPressed: () {
                          controller.loadAppointmentForEdit(appt);
                          Get.to(() => BookAppointmentPage(
                            isEditing: true,
                            existingAppointment: appt,
                          ));
                        },
                      ),
                      IconButton(
                        icon: const Icon(Icons.delete, color: Colors.red),
                        onPressed: () {
                          Get.defaultDialog(
                            title: "Delete Appointment",
                            middleText:
                            "Are you sure you want to delete this appointment?",
                            textConfirm: "Yes",
                            textCancel: "No",
                            confirmTextColor: Colors.white,
                            onConfirm: () {
                              controller.deleteAppointment(appt);
                              Get.back();
                            },
                          );
                        },
                      ),
                    ],
                  ),
                ),
              );
            },
          );
        }),
      ],
    ),
    ),

    );
  }
}
