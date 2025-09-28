import { Injectable } from '@angular/core';
import { ToastMessageOptions } from 'primeng/api';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'platform',
})
export class InternalToastService {
  messages: ToastMessageOptions[] = [];
  private messagesSubject = new BehaviorSubject<ToastMessageOptions[]>([]);
  messages$ = this.messagesSubject.asObservable();

  constructor() {
    this.initMessages();
  }

  private initMessages() {
    //TODO different applications/requests should maybe trigger some notifications for a user when they login
  }

  addMessage(message: ToastMessageOptions, sticky = false, life = 10) {
    const idx = this.messages.findIndex(x => x.id === message.id);
    if (idx != -1) return; //do not push to active messages
    message.sticky = sticky;
    message.data = { progress: life };
    message.key = 'p1s-toast'; //There may be other toast components so we map this service to a specific toast component
    if (sticky === false) {
      message.life = life * 1000;
      const interval = setInterval(() => {
        message.data.progress = message.data.progress + 100 / life;
        if (message.data.progress >= 100) {
          message.data.progress = 100;
          clearInterval(interval);
        }
      }, 1000);
    }
    this.messages.push(message);
    this.messagesSubject.next(this.messages);
  }

  removeMessage(message: ToastMessageOptions) {
    const idx = this.messages.findIndex(x => x.id === message.id);
    if (idx === -1) return;
    this.messages.splice(idx, 1);
    this.messagesSubject.next(this.messages);
  }
}
