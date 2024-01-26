using UnityEngine;

public class Death : StateMachineBehaviour
{
    public bool reward = true; 

    public Inventory playerInventory;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (reward)
        {
            GenerateLoot();
        }
        animator.gameObject.SendMessageUpwards("UnregisterEnemy", null);
        animator.gameObject.SetActive(false);
        Destroy(animator.gameObject);
    }

    //Add potions and coins to the inventory with a predetermined chance
    private void GenerateLoot()
    {
        int auxChance = Random.Range(1, 11);
        if (auxChance >= 1 && auxChance <= 5)
        {
            playerInventory.coins += 2;
        }
        else if (auxChance >= 9)
        {
            playerInventory.coins += 4;
        }
        if (auxChance == 3)
        {
            playerInventory.potions += 1;
        }
    }
    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
