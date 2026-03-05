# Copilot Instructions - Universal Software Engineering Template

- This document defines universal engineering rules and best practices.
- If a file named `project-instructions.md` exists in the same folder as this file, follow it in addition to this file for project-specific rules, architecture, constraints, bussiness logic and domain context.  
- If conflicts exist, project-specific instructions always take precedence.

---

## Decision Priorities (in order)

1. Correctness and security
2. Clarity and maintainability
3. Simplicity
4. Performance

---

## Communication Protocol

- Address the user directly with the title "Desarrollador"

---

## Architecture Guidelines

- Always enforce clear separation of concerns
- Keep business/domain logic isolated from infrastructure, frameworks, and I/O
- Never place business logic in interface or delivery layers
- Make data flow explicit between layers and modules
- Prefer stateless services when possible
- Ensure dependencies flow inward (core must not depend on outer layers)
- Design modules with high cohesion and minimal coupling
- Define clear responsibilities for each layer
- Introduce abstractions only when they reduce complexity or duplication

---

## Development Principles

### SOLID Principles
- Apply Single Responsibility consistently
- Design for extension while avoiding modification of stable code
- Preserve substitutability in inheritance and interfaces
- Keep interfaces focused and minimal
- Depend on abstractions, not concrete implementations

### Core Practices

- Follow official best practices and conventions of each language and framework
- Avoid duplication (DRY)
- Prefer the simplest solution that works (KISS)
- Do not build unused or speculative features (YAGNI)
- Minimize surprise in behavior and APIs
- Prefer composition over inheritance
- Favor immutability when practical
- Avoid known security flaws, performance traps, and anti-patterns

---

## Code Quality Rules

- Readability over cleverness
- Explicit over implicit behavior
- Consistency across the codebase
- Small focused functions/modules
- Meaningful naming everywhere
- Avoid hidden side effects

---

## Error Handling

- Validate inputs at system boundaries
- Fail fast on invalid state
- Separate domain/business errors from system failures
- Do not leak internal details to clients
- Provide consistent error response format

---

## Security Basics

- Never trust external input
- Validate and sanitize all inputs
- Sanitize outputs
- No secrets in source code
- Never log sensitive data
- Principle of least privilege

---

## Concurrency & Data Integrity

- Operations must be idempotent when possible
- Protect critical sections from race conditions
- Use transactions for multi-step state changes
- Avoid shared mutable state
- Explicitly handle concurrent writes and retries

---

## Testing Strategy

- Unit tests for business rules and core logic
- Integration tests for critical flows
- Avoid meaningless or trivial tests
- Focus on edge cases and failure scenarios
- Maintain test readability

---

## Performance Philosophy

- Measure before optimizing
- Prefer simple solutions first
- Avoid hidden expensive operations
- Optimize only proven bottlenecks
- Design with reasonable scalability in mind

---

## Logging & Observability

- Use structured logging
- Include contextual identifiers (request id, execution id, entity ids)
- Log at appropriate levels
- Avoid excessive noise
- Logs should support debugging and monitoring

---

## Anti-Patterns to Avoid

- God Objects
- Spaghetti Code
- Hardcoded configuration
- Magic numbers/strings
- Premature optimization
- Copy-paste programming
- Leaky abstractions
- Shotgun surgery
- Hidden side effects
