from transformers import AutoModelForMaskedLM, AutoTokenizer
import torch

model_id = "naver/splade-cocondenser-ensembledistil"
tokenizer = AutoTokenizer.from_pretrained(model_id)
model = AutoModelForMaskedLM.from_pretrained(model_id)

def compute_vector(text):
    tokens = tokenizer(text, return_tensors="pt")
    output = model(**tokens)
    logits, attention_mask = output.logits, tokens.attention_mask
    relu_log = torch.log(1 + torch.relu(logits))
    weighted_log = relu_log * attention_mask.unsqueeze(-1)
    max_val, _ = torch.max(weighted_log, dim=1)
    vec = max_val.squeeze()

    return vec

def get_indices_and_values(text: str):
    vec = compute_vector(text)
    indices = vec.nonzero().numpy().flatten()
    values = vec.detach().numpy()[indices]
    return indices, values