import { Pipe, PipeTransform } from "@angular/core";

@Pipe(

    {
        name: 'convertToSettings'
    }
)

export class ConvertToSettings implements PipeTransform {

    transform(value: any) {
        return "Settings"
    }

}