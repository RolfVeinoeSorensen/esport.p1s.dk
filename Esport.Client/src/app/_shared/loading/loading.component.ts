import { ChangeDetectionStrategy, ChangeDetectorRef, Component, inject } from '@angular/core';
import { transition, style, animate, trigger } from '@angular/animations';
import { LoadingHandlerService } from '@shared/loading/loading-handler.service';

const enterTransition = transition(':enter', [
  style({
    opacity: 0,
  }),
  animate(
    '0.2s ease-in',
    style({
      opacity: 1,
    })
  ),
]);

const leaveTrans = transition(':leave', [
  style({
    opacity: 1,
  }),
  animate(
    '0.5s ease-out',
    style({
      opacity: 0,
    })
  ),
]);

const fadeLoadingIn =  trigger('fadeLoadingIn', [enterTransition]);

const fadeLoadingOut = trigger('fadeLoadingOut', [leaveTrans]);

@Component({
  selector: 'app-loading',
  templateUrl: './loading.component.html',
  styleUrls: ['./loading.component.css'],
  animations: [fadeLoadingIn, fadeLoadingOut],
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [],
})
export class LoadingComponent {
  loadingHandler = inject(LoadingHandlerService);
  private cdRef = inject(ChangeDetectorRef);

  spinnerActive: boolean = true;
  constructor() {
    this.loadingHandler.showSpinner.subscribe(this.showSpinner.bind(this));
  }

  showSpinner = (state: boolean): void => {
    this.spinnerActive = state;
    this.cdRef.markForCheck();
  };
}
