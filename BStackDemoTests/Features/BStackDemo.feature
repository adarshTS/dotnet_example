Feature: BStackDemo Shopping
    As a user
    I want to filter products and add them to cart
    So that I can purchase items

Scenario: Filter by Google and add product to cart
    Given I am on the BStackDemo homepage
    When I filter products by "Google"
    And I add the first product to cart
    Then the product should be added successfully

# Scenario: Skip test for analytics tracking
#     Given I am testing skip reporting
#     When I intentionally skip this test
#     Then this test should be marked as skipped
