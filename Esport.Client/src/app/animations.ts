import { trigger, transition, animate, style } from '@angular/animations';

// Routable animations
export const slideInAnimation = trigger('routeAnimation', [
  transition('pages <=> page', [
    style({ opacity: 0, transform: 'translate3d(-10%, 0, 0)' }),
    animate('1000ms', style({ opacity: 1, transform: 'translate3d(0, 0, 0)' })),
  ]),
]);
