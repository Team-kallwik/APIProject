import 'package:assignment/Pages/Dashboard_page.dart';
import 'package:assignment/Pages/register_page.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';

class LoginPage extends StatefulWidget {
  @override
  State<LoginPage> createState() => _LoginPageState();
}

class _LoginPageState extends State<LoginPage> {
  final emailController = TextEditingController();
  final passwordController = TextEditingController();

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: Colors.white,
      body: SingleChildScrollView(
        padding: const EdgeInsets.all(24.0),
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            SizedBox(height: 80),
            // Optional logo
            Image.asset('assets/images/logo.png', height: 100,fit: BoxFit.cover,),
            SizedBox(height: 20),
            Text(
              "Login to continue",
              style: TextStyle(fontSize: 26, fontWeight: FontWeight.bold),
            ),

            SizedBox(height: 40),
            TextField(
              controller: emailController,
              keyboardType: TextInputType.emailAddress,
              decoration: InputDecoration(
                labelText: 'Email',
                prefixIcon: Icon(Icons.email_outlined),
                  labelStyle: TextStyle(color: Colors.black,fontSize: 15),
                focusedBorder: OutlineInputBorder(
                  borderSide: BorderSide(color: Colors.teal),
                ),
                border: OutlineInputBorder()
              ),
            ),
            SizedBox(height: 20),
            TextField(
              controller: passwordController,
              obscureText: true,
              decoration: InputDecoration(
                labelStyle: TextStyle(color: Colors.black,fontSize: 15),
                labelText: 'Password',
                prefixIcon: Icon(Icons.lock_outline),
                  focusedBorder: OutlineInputBorder(
                    borderSide: BorderSide(color: Colors.teal),
                  ),
                  border: OutlineInputBorder()
              ),
            ),
            SizedBox(height: 10),
            Align(
              alignment: Alignment.centerRight,
              child: TextButton(
                onPressed: () {
                  // TODO: Implement forgot password
                },
                child: Text("Forgot Password?",style: TextStyle(color: Colors.black,fontSize: 15)),
              ),
            ),
            SizedBox(height: 10),
            SizedBox(
              width: double.infinity,
              child: ElevatedButton(
                style: ElevatedButton.styleFrom(
                  backgroundColor: Colors.teal,
                  shape: RoundedRectangleBorder(
                    borderRadius: BorderRadius.circular(12),
                  ),
                ),
                onPressed: () {
                 Get.to(DashboardPage());
                 Get.snackbar("Log In",colorText: Colors.white, "Login Successful",snackStyle: SnackStyle.FLOATING,snackPosition: SnackPosition.BOTTOM,backgroundColor: Colors.blueAccent);
                },
                child: Padding(
                  padding: const EdgeInsets.symmetric(vertical: 12.0),
                  child: Text("Login", style: TextStyle(fontSize: 19,color: Colors.white)),
                ),
              ),
            ),
            SizedBox(height: 20),
            Divider(),
            TextButton(
              onPressed: () {
                Get.to(RegisterPage());
              },
              child: Text("Don't have an account? Register"),
            ),
            SizedBox(height: 40),
          ],
        ),
      ),
    );
  }
}
