import { ChangeDetectionStrategy, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
    selector: 'ui-switch-button',
    templateUrl: './switch-button.component.html',
    styleUrls: ['./switch-button.component.scss']
})
export class SwitchButtonComponent {
    @Input() isChecked: boolean = false;
    @Output() onCheckedChange = new EventEmitter<boolean>();
    constructor() { }

    ngOnInit(): void {
    }

    change($event) {
        //console.log($event.target.checked);
        this.onCheckedChange.emit($event);
    }
}
