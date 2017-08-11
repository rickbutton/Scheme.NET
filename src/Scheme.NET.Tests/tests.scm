(test #t (eqv? #t #t))
(test #t (eqv? #f #f))
(test #f (eqv? #t #f))

(test #t (eqv? 'test 'test))
(test #t (eqv? 'tEst 'test))
(test #f (eqv? 'test 'foo))

(test #t (eqv? 1 1))
(test #t (eqv? 1.0 1.0))
(test #t (eqv? 1/2 1/2))
(test #t (eqv? 1+1i 1+1i))
(test #t (eqv? 1.0+1.0i 1.0+1.0i))
(test #t (eqv? 1/2+1/2i 1/2+1/2i))

(test #f (eqv? 1.0 1))
(test #f (eqv? 0.5 1/2))
(test #f (eqv? 1.0+1.0i 1+1i))
(test #f (eqv? 0.5+0.5i 1/2+1/2i))

(test #f (eqv? 1 2))
(test #f (eqv? 1.0 2.0))
(test #f (eqv? 1/2 2/2))
(test #f (eqv? 1+1i 2+1i))
(test #f (eqv? 1+1i 1+2i))
(test #f (eqv? 1.0+1.0i 2.0+1.0i))
(test #f (eqv? 1.0+1.0i 1.0+2.0i))
(test #f (eqv? 1/2+1/2i 2/2+1/2i))
(test #f (eqv? 1/2+1/2i 1/2+2/2i))

(test #t (eqv? #\a #\a))
(test #f (eqv? #\a #\z))
(test #t (eqv? #\z #\z))

(test #t (eqv? () ()))


(define a (1 . 2))
(define a2 (1 . 2))
(define b #(1 2))
(define b2 #(1 2))
(define c "test")
(define c2 "test")

(test #t (eqv? a a))
(test #f (eqv? a a2))
(test #t (eqv? b b))
(test #f (eqv? b b2))
(test #t (eqv? c c))
(test #f (eqv? c c2))
 
(test #t (eqv? + +))
(test #f (eqv? + -))

; I'm super lazy so and its still in spec for 
; these to actually just be the same procedure
; so this test ensures that when I eventually change
; it I will remember to properly write tests for it
(test #t (eqv? eqv? eq?))

(test #t (equal? #t #t))
(test #t (equal? #f #f))
(test #f (equal? #t #f))

(test #t (equal? 'test 'test))
(test #t (equal? 'tEst 'test))
(test #f (equal? 'test 'foo))

(test #t (equal? 1 1))
(test #t (equal? 1.0 1.0))
(test #t (equal? 1/2 1/2))
(test #t (equal? 1+1i 1+1i))
(test #t (equal? 1.0+1.0i 1.0+1.0i))
(test #t (equal? 1/2+1/2i 1/2+1/2i))

(test #f (equal? 1.0 1))
(test #f (equal? 0.5 1/2))
(test #f (equal? 1.0+1.0i 1+1i))
(test #f (equal? 0.5+0.5i 1/2+1/2i))

(test #f (equal? 1 2))
(test #f (equal? 1.0 2.0))
(test #f (equal? 1/2 2/2))
(test #f (equal? 1+1i 2+1i))
(test #f (equal? 1+1i 1+2i))
(test #f (equal? 1.0+1.0i 2.0+1.0i))
(test #f (equal? 1.0+1.0i 1.0+2.0i))
(test #f (equal? 1/2+1/2i 2/2+1/2i))
(test #f (equal? 1/2+1/2i 1/2+2/2i))

(test #t (equal? #\a #\a))
(test #f (equal? #\a #\z))
(test #t (equal? #\z #\z))

(test #t (equal? () ()))


(test #t (equal? a a))
(test #t (equal? a a2))
(test #t (equal? b b))
(test #t (equal? b b2))
(test #t (equal? c c))
(test #t (equal? c c2))

(test #t (equal? + +))
(test #f (equal? + -))