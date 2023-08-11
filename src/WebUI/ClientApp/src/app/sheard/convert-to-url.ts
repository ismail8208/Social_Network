import { Pipe, PipeTransform } from "@angular/core";

@Pipe(

    {
        name: 'convertToUrl'
    }
)

export class ConvertToUrl implements PipeTransform {

    transform(value: any, root: string) {
        return  root+value+"/Video"
    }

}