from sparse import get_indices_and_values
from fastapi import FastAPI
from pydantic import BaseModel

class RequestPayload(BaseModel):
    text: str


app = FastAPI()


@app.post("/")
def create_sparse(payload: RequestPayload):
    indices, values = get_indices_and_values(payload.text)
    return { "indices": indices.tolist(), "values": values.tolist() }

@app.get("/healthcheck")
def health():
    return "OK"