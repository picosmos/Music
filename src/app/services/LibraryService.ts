import { Injectable } from "@angular/core";
import * as Datastore from "nedb";

@Injectable()
export class LibraryService {
    constructor() {
        this._db = new Datastore({ filename: 'datafile.nedb', autoload: true });
    }

    private _db: Datastore;

    public addPath(path: string): void {
        this._db.insert(new PathModel(path));
    }

    public async deletePath(path: string): Promise<number> {
        return new Promise<number>(resolve => {
            this._db.remove(new PathModel(path), (err, n) => {
                resolve(n);
            });
        });
    }

    public async getPaths(): Promise<string[]> {
        return new Promise<string[]>(resolve => {
            this._db.find({ type: "path" }, (err, docs: PathModel[]) => {
                let result = [];
                for (let path of docs) {
                    result.push(path.path);
                }
                resolve(result);
            });
        });
    }
}

export interface IModel {
    type: string;
}

export class PathModel implements IModel {
    public get type(): string { return "path" }

    public constructor(
        public path: string
    ) {
    }
}