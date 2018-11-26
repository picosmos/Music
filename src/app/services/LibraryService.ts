import { Injectable } from "@angular/core";
import * as Datastore from "nedb";

@Injectable()
export class LibraryService {
    constructor() {
        this._db = new Datastore({ filename: 'datafile.nedb', autoload: true });
    }

    private _db: Datastore;

    public addPath(path: string): Promise<void> {
        return new Promise((resolve, reject) => {
            try {
                this._db.insert(new PathModel(path), (err, document) => {
                    if (err) {
                        reject(err);
                    }
                    else {
                        resolve();
                    }
                });
            }
            catch (ex) {
                reject(ex);
            }
        });
    }

    public async deletePath(path: string): Promise<number> {
        return new Promise<number>((resolve, reject) => {
            try {
                this._db.remove(new PathModel(path), (err, n) => {
                    if (err) {
                        reject(err);
                    }
                    else {
                        resolve(n);
                    }
                });
            }
            catch (ex) {
                reject(ex);
            }
        });
    }

    public async getPaths(): Promise<string[]> {
        return new Promise<string[]>((resolve, reject) => {
            try {
                this._db.find({ type: "path" }, (err, docs: PathModel[]) => {
                    if (err) {
                        reject(err);
                        return;
                    }
                    let result = [];
                    for (let path of docs) {
                        result.push(path.path);
                    }
                    resolve(result);
                });
            }
            catch (ex) {
                reject(ex);
            }
        });
    }
}

export interface IModel {
    type: string;
}

export class PathModel implements IModel {
    public type = "path";

    public constructor(
        public path: string
    ) {
    }
}