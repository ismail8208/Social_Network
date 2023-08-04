import { Pipe, PipeTransform } from "@angular/core";

@Pipe(

    {
        name: 'converToRoot'
    }
)

export class converToRoot implements PipeTransform {

    transform(value: any, root: string) {
        return value.replace("wwwroot",root);
    }

}