import 'package:flutter/material.dart';
import 'core/theme/app_theme.dart';
import 'shared/widgets/custom_button.dart';
import 'shared/widgets/custom_text_field.dart';

void main() => runApp(const TestApp());

class TestApp extends StatelessWidget {
  const TestApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      theme: AppTheme.light,
      home: Scaffold(
        appBar: AppBar(title: const Text('Theme test')),
        body: Padding(
          padding: const EdgeInsets.all(16),
          child: Column(
            children: [
              const CustomTextField(label: 'Email'),
              const SizedBox(height: 16),
              CustomButton(label: 'Login', onPressed: () {}),
            ],
          ),
        ),
      ),
    );
  }
}