import 'package:flutter/material.dart';

class ImageViewerPage extends StatelessWidget {
  const ImageViewerPage({super.key});

  @override
  Widget build(BuildContext context) {
    const imageUrl = 'https://picsum.photos/1600/900';

    return Scaffold(
      appBar: AppBar(
        title: const Text('Image Viewer'),
      ),
      body: Center(
        child: Image.network(
          imageUrl,
          fit: BoxFit.cover,
          loadingBuilder: (context, child, loadingProgress) {
            if (loadingProgress == null) return child;
            return const CircularProgressIndicator();
          },
          errorBuilder: (context, error, stackTrace) {
            return const Text(
              'Failed to load image',
              style: TextStyle(color: Colors.red),
            );
          },
        ),
      ),
    );
  }
}
