import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import * as signalR from '@microsoft/signalr';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  imports:[FormsModule, CommonModule],
})
export class AppComponent implements OnInit {
  user = 'User 2'; // change manually to 'User2' in second tab/port
  message = '';
  messages: string[] = [];
  private hubConnection!: signalR.HubConnection;
  selectedMode: string = 'ai'; // Default to Chat with AI

  isConnected = false;

  ngOnInit() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7183/chathub')
      .withAutomaticReconnect()
      .build();

    this.hubConnection.on('ReceiveMessage', (user, message) => {
      this.messages.push(`${user}: ${message}`);
    });

    this.hubConnection
      .start()
      .then(() => {
        console.log('SignalR Connected');
        this.isConnected = true;   // connection ready
      })
      .catch(err => console.error('Connection error:', err));
  }

  sendMessage() {
    if (!this.isConnected) {
      alert('Connection not established yet. Please wait.');
      return;
    }
    if (this.selectedMode === 'ai') {
      if (this.message) {
        this.hubConnection.invoke('SendMessageWithAi', this.user, this.message)
          .catch(err => console.error(err));
        this.message = '';
      }
    } else {
      if (this.message) {
        this.hubConnection.invoke('SendMessage', this.user, this.message)
          .catch(err => console.error(err));
        this.message = '';
      }
    }
  }
}
