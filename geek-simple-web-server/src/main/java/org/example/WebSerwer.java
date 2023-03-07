package org.example;


import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.PrintWriter;
import java.net.ServerSocket;
import java.net.Socket;
import java.nio.charset.StandardCharsets;

public class WebSerwer {
    public static void main(String[] args) {
        try (ServerSocket ServerSocket = new ServerSocket(8088)) {

            while (true) {
                Socket Socket = ServerSocket.accept();
                System.out.println("New client connected!");

                BufferedReader input = new BufferedReader(
                        new InputStreamReader(
                                Socket.getInputStream(), StandardCharsets.UTF_8));
                PrintWriter output = new PrintWriter(Socket.getOutputStream());

                while (!input.ready()) ;

                while (input.ready()) {
                    System.out.println(input.readLine());
                }

                output.println("HTTP/1.1 200 OK");
                output.println("Content-Type: text/html charset=utf-8");
                output.println();
                output.println("<h1>Hello from server!</h1>");
                output.flush();

                input.close();
                output.close();
            }
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}
