import 'package:flutter/material.dart';
import 'FullScreenImageViewer.dart';
import 'PDFViewerPage.dart';
import 'EditRecordPage.dart';

class RecordDetailsPage extends StatefulWidget {
  final String recordType;
  final int recordNumber;

  const RecordDetailsPage(this.recordType, this.recordNumber, {super.key});

  @override
  State<RecordDetailsPage> createState() => _RecordDetailsPageState();
}
class _RecordDetailsPageState extends State<RecordDetailsPage> {
  String doctorName = 'Dr. Shivani Patel';
  String hospitalName = 'MediCare Health & Diagnostics';
  String notes =
      'Patient advised to rest and avoid heavy physical activity. Complete 5-day course of medication and follow-up required next week.';

  final dummyImages = [
    'https://picsum.photos/id/1015/1600/900',
    'https://picsum.photos/id/1025/1600/900',
    'https://picsum.photos/id/1036/1600/900',
  ];


  final dummyPdfUrl =
      'https://cdn.syncfusion.com/content/PDFViewer/flutter-succinctly.pdf';

  void _confirmDelete(BuildContext context) {
    showDialog(
      context: context,
      builder: (_) => AlertDialog(
        title: const Text('Delete Record'),
        content: const Text('Are you sure you want to delete this record?'),
        actions: [
          TextButton(
            child: const Text('Cancel'),
            onPressed: () => Navigator.pop(context),
          ),
          TextButton(
            child: const Text('Delete', style: TextStyle(color: Colors.red)),
            onPressed: () {
              Navigator.pop(context);
              ScaffoldMessenger.of(context).showSnackBar(
                const SnackBar(content: Text('Record deleted')),
              );
              Navigator.pop(context); // Back to list
            },
          ),
        ],
      ),
    );
  }

  void _openEditPage() async {
    final result = await Navigator.push(
      context,
      MaterialPageRoute(
        builder: (_) => EditRecordPage(
          doctorName: doctorName,
          hospitalName: hospitalName,
          notes: notes,
        ),
      ),
    );

    if (result != null && result is Map<String, String>) {
      setState(() {
        doctorName = result['doctor']!;
        hospitalName = result['hospital']!;
        notes = result['notes']!;
      });

      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(content: Text("Record updated")),
      );
    }
  }
  @override
  Widget build(BuildContext context) {
    final recordTitle =
        '${widget.recordType} Record ${widget.recordNumber}';
    return Scaffold(
      appBar: AppBar(
        title: Text(recordTitle, style: const TextStyle(color: Colors.white)),
        backgroundColor: Colors.teal,
        leading: const BackButton(color: Colors.white),
        actions: [
          IconButton(
            icon: const Icon(Icons.edit, color: Colors.white),
            onPressed: _openEditPage,
          ),
          IconButton(
            icon: const Icon(Icons.delete, color: Colors.white),
            onPressed: () => _confirmDelete(context),
          ),
        ],
      ),
      body: SingleChildScrollView(
        padding: const EdgeInsets.all(16.0),
        child: Card(
          elevation: 4,
          shape:
          RoundedRectangleBorder(borderRadius: BorderRadius.circular(12)),
          child: Padding(
            padding: const EdgeInsets.all(20),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(
                  recordTitle,
                  style: const TextStyle(
                    fontSize: 22,
                    fontWeight: FontWeight.bold,
                    color: Colors.teal,
                  ),
                ),
                const SizedBox(height: 8),
                const Text(
                  'Added on 27 July 2025',
                  style: TextStyle(fontSize: 14, color: Colors.grey),
                ),
                const Divider(height: 30),

                const Text('Doctor:', style: TextStyle(fontWeight: FontWeight.bold)),
                Text(doctorName),
                const SizedBox(height: 10),

                const Text('Hospital:', style: TextStyle(fontWeight: FontWeight.bold)),
                Text(hospitalName),
                const Divider(height: 30),

                const Text(
                  'Doctor\'s Notes:',
                  style: TextStyle(fontWeight: FontWeight.bold, fontSize: 16),
                ),
                const SizedBox(height: 8),
                Text(notes, style: const TextStyle(fontSize: 15)),

                const Divider(height: 30),

                const Text('Report Document:',
                    style:
                    TextStyle(fontWeight: FontWeight.bold, fontSize: 16)),
                const SizedBox(height: 10),
                ElevatedButton.icon(
                  onPressed: () {
                    Navigator.push(
                      context,
                      MaterialPageRoute(
                        builder: (_) => PDFViewerPage(url: dummyPdfUrl),
                      ),
                    );
                  },
                  icon: const Icon(Icons.picture_as_pdf),
                  label: const Text("View / Download PDF"),
                  style: ElevatedButton.styleFrom(
                    backgroundColor: Colors.teal,
                    foregroundColor: Colors.white,
                  ),
                ),

                const Divider(height: 30),

                const Text('Attached Images:',
                    style:
                    TextStyle(fontWeight: FontWeight.bold, fontSize: 16)),
                const SizedBox(height: 10),
                GridView.builder(
                  shrinkWrap: true,
                  physics: const NeverScrollableScrollPhysics(),
                  itemCount: dummyImages.length,
                  gridDelegate: const SliverGridDelegateWithFixedCrossAxisCount(
                    crossAxisCount: 3,
                    mainAxisSpacing: 8,
                    crossAxisSpacing: 8,
                    childAspectRatio: 1,
                  ),
                  itemBuilder: (context, index) {
                    return GestureDetector(
                      onTap: () {
                        Navigator.push(
                          context,
                          MaterialPageRoute(
                            builder: (_) => FullScreenImageViewer(
                              imageUrl: dummyImages[index],
                            ),
                          ),
                        );
                      },
                      child: ClipRRect(
                        borderRadius: BorderRadius.circular(8),
                        child: Image.network(dummyImages[index],
                            fit: BoxFit.cover),
                      ),
                    );
                  },
                ),
              ],
            ),
          ),
        ),
      ),
    );
  }
}
